using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_player : MonoBehaviour {

    //Component Variables
    Transform trans;
    SpriteRenderer sr;
    scr_placeMeeting pm;
    scr_physicsObject po;

    //Create Other Player Controlled Game Objects
    public GameObject attackObject; //(AttackObject and Arm Seperate)
    GameObject playerAttack;
    public GameObject blockObject;
    GameObject playerBlock;
    public GameObject armObject;
    GameObject armInst;

    //GameObject playerAttack;
    //playerAttack = Object.Instantiate(playerAttack, new Vector3(0,0,0), Quaternion.identity);

    //Player Variables
    float alpha = 1f;
    public int hDir = 1;
    float moveSpeed = .09f; //.1f
    float jumpSpeed = .16f;
    int jumpAlarm = 0;
    int jumpDelay = 5;
    int eJumpAlarm = 0;
    int eJumpTime = 17;
    //float width = 
    //float height = 

    //Player Attack/Block Variables
    //float attackOffSet = 1.25f;
    int actionAlarm = 0;
    int attackDuration = 4;
    int attackTime = 40;
    int blockDuration = 12;
    int blockTime = 40;

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

    //Player Element Dimension Variables
    float width;
    float height;
    float aWidth; //(Attack Dimensions)
    float aHeight;
    float armWidth; //(Arm Dimensions)
    float armHeight;
    float bWidth;
    float bHeight;

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
        //Create Player Attack/Block Instances
        playerAttack = Instantiate(attackObject, new Vector3(0, 0, 0), Quaternion.identity);
        playerBlock = Instantiate(blockObject, new Vector3(0, 0, 0), Quaternion.identity);

        //Create Player Arm Inst
        armInst = Instantiate(armObject, new Vector3(0, 0, 0), Quaternion.identity);
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
        bool block = Input.GetKey(KeyCode.X);

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
                if (!playerAttack.activeSelf && !playerBlock.activeSelf)
                {
                    hDir = 1;
                }
            }
            else if (left)
            {
                po.hSpeed = -moveSpeed;

                //Switch Direction
                if (!playerAttack.activeSelf && !playerBlock.activeSelf)
                {
                    hDir = -1;
                }
            }
            else po.hSpeed = 0;
        }

        //Initiate Attack
        if(attack && actionAlarm <= 0 && !stunned)
        {
            //Reactivate Player Attack
            actionAlarm = attackTime;
            playerAttack.SetActive(true);

            //Reset playerAttack usedCollideList
            playerAttack.GetComponent<scr_playerAttack>().ResetCollideArray();

            //Set Proper Position To Ensure Proper Collision (MAKE SURE CODE IS EQUIVALENT TO THAT IN THE LATEUPDATE FUNCTION)
            playerAttack.transform.position = new Vector3(trans.position.x + hDir * (width/2 + aWidth/2), trans.position.y, trans.position.z);
        }
        //End Attack
        else if (playerAttack.activeSelf && actionAlarm < attackTime - attackDuration)
        {
            //Deactivate Instance 
            playerAttack.SetActive(false);
        }

        //Initiate Block
        if(block && !attack && actionAlarm <= 0 && !stunned)
        {
            //Reactivate Player Block
            actionAlarm = blockTime;
            playerBlock.SetActive(true);

            //Set Proper Position (Copied from attack initiation)
            playerAttack.transform.position = new Vector3(trans.position.x + hDir * (width / 2 + bWidth / 2), trans.position.y, trans.position.z);
        }
        //End Block
        else if(playerBlock.activeSelf && actionAlarm < blockTime - blockDuration)
        {
            //Deactivate Instance
            playerBlock.SetActive(false);
        }

        //Deduct Action Alarm
        if (actionAlarm > 0)
        {
            actionAlarm -= 1;
        }

        //Manage Sprite Flipping
        if (hDir == 1) //RIGHT
        {
            sr.flipX = false;
            playerAttack.GetComponent<SpriteRenderer>().flipX = false;
            playerBlock.GetComponent<SpriteRenderer>().flipX = false;
            armInst.GetComponent<SpriteRenderer>().flipX = false;
        }
        else           //LEFT
        {
            sr.flipX = true;
            playerAttack.GetComponent<SpriteRenderer>().flipX = true;
            playerBlock.GetComponent<SpriteRenderer>().flipX = true;
            armInst.GetComponent<SpriteRenderer>().flipX = true;
        }

        //Player Get Damaged
        if(pm.PlaceMeeting(trans.position.x,trans.position.y,2))
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

        //Arm Animation Manager
        if(armInst.activeSelf)
        {

        }
    }

    //POST PHYSICS OBJECT CALCULATIONS
    private void LateUpdate()
    {
        //Update Player Dimensions
        width = sr.bounds.size.x;
        height = sr.bounds.size.y;

        //Manage Attack Instance
        if (playerAttack.activeSelf)
        {
            aWidth = playerAttack.GetComponent<SpriteRenderer>().bounds.size.x;
            aHeight = playerAttack.GetComponent<SpriteRenderer>().bounds.size.y;
            playerAttack.transform.position = new Vector3(trans.position.x + hDir * (width/2+aWidth/2), trans.position.y, trans.position.z);
        }

        //Manage Block Instance
        if(playerBlock.activeSelf)
        {
            //Debug.Log("BLLEPEEPP");
            bWidth = playerBlock.GetComponent<SpriteRenderer>().bounds.size.x;
            bHeight = playerBlock.GetComponent<SpriteRenderer>().bounds.size.y;
            playerBlock.transform.position = new Vector3(trans.position.x + hDir * (width / 2 + bWidth / 2), trans.position.y, trans.position.z);
        }

        //Manage Player Arm Instance
        armWidth = armInst.GetComponent<SpriteRenderer>().bounds.size.x;
        armHeight = armInst.GetComponent<SpriteRenderer>().bounds.size.y;
        armInst.transform.position = new Vector3(trans.position.x + hDir * (width/2 + armWidth/2), trans.position.y, trans.position.z);

        //Modify Additional Instance Scales (Scale of playerAttack is controlled within it's scripts) 
        //NOTE: THESE SCALES ARE RELATIVE TO THE OBJECTS SPRITE. MUST USE SAME SPRITE TO WORK PROPERLY.
        playerBlock.GetComponent<Transform>().localScale = new Vector3(trans.lossyScale.x, trans.lossyScale.y, trans.lossyScale.z);
        armInst.GetComponent<Transform>().localScale = new Vector3(trans.lossyScale.x, trans.lossyScale.y, trans.lossyScale.z);

        //DEBUGGING Set Player Arm Opacity (Color is changed as well)
        armInst.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
    }

    public void takeDamage()
    {
        //Only Damage If Not Invulnerable
        if (!invulnerable)
        {
            //Stun Player
            stunned = true;
            stunAlarm = stunTime;

            //Make Player Invulnerable
            invulnerable = true;
            invulnAlarm = invulnTime;

            //Give Player Knockback
            po.vSpeed = vKnockback;
            po.hSpeed = hKnockback * -hDir;
        }
    }
}
