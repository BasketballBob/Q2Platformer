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
    float moveSpeed = .04f; //.05f
    float jumpSpeed = .25f;
    float moveDir = 1f;

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

    }

    // Update is called once per frame
    void Update()
    {

        //Become Active Once On Screen
        if (!active && Mathf.Abs(mainCam.position.x - trans.position.x) < scr_cameraBehavior.width / 2)
        {
            active = true;
        }

        //Activated AI Behavior
        if (active && !enemy.stunned)
        {
            //Determine Movement Destination 
            float moveBarrier = 2f; //Added Distance Between Player and Enemy
            float moveDest = player.position.x - ((GetComponent<SpriteRenderer>().bounds.size.x/2 + 
            player.gameObject.GetComponent<SpriteRenderer>().bounds.size.x/2 + moveBarrier) * pm.Sign(player.position.x - trans.position.x));

            //Determine Movement Dir 
            moveDir = pm.Sign(moveDest - trans.position.x);

            //Move Towards Destination
            if (moveDir == pm.Sign(moveDest + moveSpeed - trans.position.x))
            {
                po.hSpeed = moveSpeed * moveDir;
            }
            else po.hSpeed = 0;

            //Check If Obstacle Is In Path
            if (pm.PlaceMeeting(trans.position.x + minMove * moveDir, trans.position.y, 0))
            {
                //Jump If Capable
                if (pm.PlaceMeeting(trans.position.x, trans.position.y - minMove, 0))
                {
                    po.vSpeed = jumpSpeed;
                }
            }
        }//AI BEHAVIOR
    }
}
