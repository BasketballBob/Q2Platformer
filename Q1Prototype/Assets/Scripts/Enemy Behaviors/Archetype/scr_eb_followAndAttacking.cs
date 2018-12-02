using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_eb_followAndAttacking : MonoBehaviour
{

    //Component References
    Transform trans;
    scr_physicsObject po;
    scr_placeMeeting pm;
    scr_enemy enemy;
    Transform player;
    Transform mainCam;

    //Following Enemy Variables
    float minMove = scr_physicsObject.minMove;
    bool active = false;
    public float moveSpeed = .04f; //.05f
    public float jumpSpeed = .25f;
    float moveDir = 1f;
    float attackDir = 1f;
    public float moveBarrier = .5f; //Added Distance Between Player and Enemy

    //Attacking Variables
    int attackAlarm = 0;
    int attackTime = 120;
    int attackDuration = 6;
    int attackWindUp = 20;

    //Dimensional Variables
    float enemyWidth;
    float attackWidth;

    //Enemy Attack Instance
    public GameObject attackObject;
    GameObject enemyAttack;

    //Define Component Variables
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        po = GetComponent<scr_physicsObject>();
        pm = GetComponent<scr_placeMeeting>();
        enemy = GetComponent<scr_enemy>();
        player = GameObject.Find("obj_player").GetComponent<Transform>();
        mainCam = GameObject.Find("obj_playerCamera").GetComponent<Transform>();
    }

    // Use this for initialization
    void Start()
    {
        //Instantiate Enemy Attack
        enemyAttack = Instantiate(attackObject, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

        //Become Active Once On Screen
        if (!active && Mathf.Abs(mainCam.position.x - trans.position.x) < scr_cameraBehavior.width / 2)
        {
            active = true;
        }

        //Walk Towards Player
        if (active && !enemy.stunned && attackAlarm <= 0)
        {
            //Determine Movement Destination 
            float moveDest = player.position.x - ((GetComponent<SpriteRenderer>().bounds.size.x / 2 +
            player.gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2 + moveBarrier) * pm.Sign(player.position.x - trans.position.x));

            //Determine Movement Dir 
            moveDir = pm.Sign(moveDest - trans.position.x);

            //Attack Dir (Edgecase For Attacking
            attackDir = pm.Sign(player.position.x - trans.position.x);

            //Move Towards Destination
            if (moveDir == pm.Sign(moveDest + moveSpeed - trans.position.x))
            {
                po.hSpeed = moveSpeed * moveDir;
            }
            //Begin Attacking When At Destination (And On Ground)
            else if (pm.PlaceMeeting(trans.position.x, trans.position.y - minMove, 0))
            {
                po.hSpeed = 0;
                attackAlarm = attackTime;
            }

            //Check If Obstacle Is In Path
            if (pm.PlaceMeeting(trans.position.x + minMove * moveDir, trans.position.y, 0))
            {
                //Jump If Capable
                if (pm.PlaceMeeting(trans.position.x, trans.position.y - minMove, 0))
                {
                    po.vSpeed = jumpSpeed;
                }
            }
        }

        //Update Attack Dir
        enemyAttack.GetComponent<scr_enemyAttack>().UpdateHDir(attackDir);

        //Initiate Attack
        if(active && !enemy.stunned && attackAlarm > (attackTime - (attackWindUp + attackDuration)))
        {
            //Freeze Enemy
            po.hSpeed = 0;
            
            //Activate Attack Inst
            if(attackAlarm == attackTime)
            {
                //enemyAttack.SetActive(true);
                //enemyAttack.GetComponent<scr_enemyAttack>().ReactivateDamage();
            }
        }
        //Deactivate Attack
        else if(enemyAttack.activeSelf && attackAlarm <= (attackTime-(attackWindUp+attackDuration)))
        {
            enemyAttack.SetActive(false);
        }

        //Attack Timeline
        if(attackAlarm > 0)
        {
            //Behavior Manager 
            if(attackAlarm == attackTime - attackWindUp) //Activate Attack
            {
                enemyAttack.SetActive(true);
                enemyAttack.GetComponent<scr_enemyAttack>().ReactivateDamage();
            }
            /*else if (attackAlarm == attackTime - (attackWindUp+attackDuration)) //Deactivate Attack
            {
                enemyAttack.SetActive(false);
            }*/

            //Countdown Alarm
            attackAlarm -= 1;
        }

        //Reset Behavior On Stun
        if(enemy.stunned)
        {
            attackAlarm = 0;
        }
    }

    private void LateUpdate()
    {
        //Update Player Dimensions
        enemyWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        //Manage Attack Object
        if(enemyAttack.activeSelf)
        {
            //Update Attack Dimensions
            attackWidth = enemyAttack.GetComponent<SpriteRenderer>().bounds.size.x;

            //Manage Attack Position
            enemyAttack.GetComponent<Transform>().position = new Vector3(trans.position.x + attackDir * (enemyWidth/2 + attackWidth/2) , trans.position.y, trans.position.z);

            //Manage Scale Of Attack
            enemyAttack.transform.localScale = trans.lossyScale;
        }
    }
}
