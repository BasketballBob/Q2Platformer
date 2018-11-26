using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_player : MonoBehaviour {

    //Component Variables
    Transform trans;
    SpriteRenderer sr;
    scr_placeMeeting pm;
    scr_physicsObject po;
    public GameObject attackObject;
    GameObject playerAttack;

    //GameObject playerAttack;
    //playerAttack = Object.Instantiate(playerAttack, new Vector3(0,0,0), Quaternion.identity);

    //Player Variables
    float alpha = 1f;
    int hDir = 1;
    float moveSpeed = .09f; //.1f
    float jumpSpeed = .16f;
    int jumpAlarm = 0;
    int jumpDelay = 5;
    int eJumpAlarm = 0;
    int eJumpTime = 17;

    //Player Attack Variables
    float attackOffSet = 1.25f;
    int attackAlarm = 0;
    int attackDuration = 4;
    int attackTime = 40;

    //Player Damage Variables
    bool stunned = false;
    bool invulnerable = false;
    float stunAlarm = 0;
    float stunTime = 20;
    float invulnAlarm = 0;
    float invulnTime = 120;
    float vKnockback = .2f;
    float hKnockback = .1f;
    float flickerMin = .5f;
    float flickerMax = 1f;
    float flickerRate = .1f;
    float flickerDir = 1f;

    //Define Component Variables
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponent<scr_placeMeeting>();
        po = GetComponent<scr_physicsObject>();
    }

    // Use this for initialization
    void Start () 
    {
        //Create Player Attack Instance
        playerAttack = Instantiate(attackObject, new Vector3(0, 0, 0), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {

        //TEST: Set Player Opacity
        //sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, .5f);

        //Define minMove
        float minMove = scr_physicsObject.minMove;

        //Check For Player Input
        bool up = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);
        bool down = Input.GetKey(KeyCode.DownArrow);
        bool right = Input.GetKey(KeyCode.RightArrow);
        bool left = Input.GetKey(KeyCode.LeftArrow);
        bool attack = Input.GetKey(KeyCode.Z);

        //Jumping 
        if (up && pm.PlaceMeeting(trans.position.x, trans.position.y - minMove, 0) && jumpAlarm <= 0 && !stunned)
        {
            po.vSpeed = jumpSpeed;
            jumpAlarm = jumpDelay;
            eJumpAlarm = eJumpTime;
        }

        //Extended Jump
        if (up && eJumpAlarm > 0)
        {
            po.vSpeed = jumpSpeed;
            eJumpAlarm -= 1;
        }
        else eJumpAlarm = 0;

        //Deduct Jump Alarm
        if (pm.PlaceMeeting(trans.position.x, trans.position.y - minMove, 0) && jumpAlarm > 0) jumpAlarm -= 1;

        //Manage Horizontal Speed
        if (!stunned)
        {
            if (right)
            {
                po.hSpeed = moveSpeed;

                //Switch Direction
                if (attackAlarm < attackTime - attackDuration)
                {
                    hDir = 1;
                }
            }
            else if (left)
            {
                po.hSpeed = -moveSpeed;

                //Switch Direction
                if (attackAlarm < attackTime - attackDuration)
                {
                    hDir = -1;
                }
            }
            else po.hSpeed = 0;
        }

        //Initiate Attack
        if(attack && attackAlarm <= 0 && !stunned)
        {
            //Reactivate Player Attack
            attackAlarm = attackTime;
            playerAttack.SetActive(true);

            //Reset playerAttack usedCollideList
            playerAttack.GetComponent<scr_playerAttack>().ResetCollideArray();

            //Set Proper Position To Ensure Proper Collision (MAKE SURE CODE IS EQUIVALENT TO THAT IN THE LATEUPDATE FUNCTION)
            playerAttack.transform.position = new Vector3(trans.position.x + hDir * attackOffSet, trans.position.y, trans.position.z);
        }
        //End Attack
        else if (playerAttack.activeSelf && attackAlarm < attackTime - attackDuration)
        {
            //Deactivate Instance 
            playerAttack.SetActive(false);
        }

        //Deduct Attack Alarm
        if (attackAlarm > 0)
        {
            attackAlarm -= 1;
        }

        //Manage Sprite Flipping
        if (hDir == 1) //RIGHT
        {
            sr.flipX = false;
            playerAttack.GetComponent<SpriteRenderer>().flipX = false; 
        }
        else           //LEFT
        {
            sr.flipX = true;
            playerAttack.GetComponent<SpriteRenderer>().flipX = true;
        }

        //Player Get Damaged
        if(pm.PlaceMeeting(trans.position.x,trans.position.y,2) && !invulnerable)
        {
            takeDamage();
        }

        //Countdown Stun Alarm
        if (pm.PlaceMeeting(trans.position.x, trans.position.y - minMove, 0))
        {
            if (stunAlarm > 0) stunAlarm -= 1;
            else if (stunAlarm <= 0) stunned = false;
        }
        //Debug.Log(stunned);
        //Debug.Log(invulnerable);

        //Countdown Invulnerability Alarm
        if(invulnerable && !stunned)
        {
            if (invulnAlarm > 0) invulnAlarm -= 1;
            else invulnerable = false;
        }

        //Stop Stunned Player Horizontal Sliding (EDGECASE CODE)
        if (pm.PlaceMeeting(trans.position.x, trans.position.y - minMove, 0) && po.vSpeed <= 0 &&  stunned)
        {
            po.hSpeed = 0;
        }

        //MANAGER PLAYER OPACITY
        if (invulnerable)
        {
            //Two Way Flickering
            if (alpha + flickerDir * flickerRate > flickerMax)
            {
                alpha = flickerMax;
                flickerDir *= -1;
            }
            else if (alpha + flickerDir * flickerRate < flickerMin)
            {
                alpha = flickerMin;
                flickerDir *= -1;
            }
            else alpha += flickerRate * flickerDir;
        }
        else alpha = 1;

        //Set Player's Alpha
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }

    //POST PHYSICS OBJECT CALCULATIONS
    private void LateUpdate()
    {
        //Manage Attack Instance
        if (playerAttack.activeSelf)
        {
            playerAttack.transform.position = new Vector3(trans.position.x + hDir * attackOffSet, trans.position.y, trans.position.z);
        }
    }

    void takeDamage()
    {
        //Stun Player
        stunned = true;
        stunAlarm = stunTime;

        //Make Player Invulnerable
        invulnerable = true;
        invulnAlarm = invulnTime;

        //Give Player Knockback
        po.vSpeed = vKnockback;
        po.hSpeed = hKnockback*-hDir;
    }
}
