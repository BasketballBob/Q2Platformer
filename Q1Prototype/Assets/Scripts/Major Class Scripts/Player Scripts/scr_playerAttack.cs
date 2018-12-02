using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_playerAttack : MonoBehaviour {

    //Component Variables
    Transform trans;
    SpriteRenderer sr;
    BoxCollider2D bc2d;
    scr_placeMeeting pm;
    ContactFilter2D contactFilter;
    Collider2D[] collideList = new Collider2D[16];
    Collider2D[] usedCollideList = new Collider2D[16]; //USED TO CHECK WHICH ENEMIES HAVE ALREADY BEEN HIT
    Transform player;

    //Player Attack Variables 
    float hitDir;
    //Vector3 scale = new Vector3(2, .6f, 1);
    Vector3 scale = new Vector3(1, .6f, 1);

    //Define Componenet Variables
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        pm = GetComponent<scr_placeMeeting>();
        player = GameObject.Find("obj_player").GetComponent<Transform>();
    }

    // Use this for initialization
    void Start () 
    {
        //Set Initial Attack Scale
        trans.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        //Manage Attack Scale
        trans.localScale = scale;

        //Check For Collisions With Enemies
        if (pm.PlaceMeeting(trans.position.x, trans.position.y, 2))
        {
            //Get List Of Colliding Enemies
            Physics2D.OverlapCollider(bc2d, contactFilter, collideList);

            //Check For All Colliding Enemies
            for (int i = 0; i < collideList.Length; i++)
            {
                //Reset Instance Checked Variable
                bool instRegistered = false;

                //Check If Collision Instance Exists
                if(collideList[i] != null)
                {
                    //Check If Collision Instance Is An Enemy
                    if(collideList[i].gameObject.GetComponent<scr_enemy>() != null)
                    {
                        //Check If Enemy Is Alive
                        if(collideList[i].gameObject.GetComponent<scr_enemy>().dead == false)
                        { 
                            //Check If Instance Has Already Been Registered 
                            for (int p = 0; p < usedCollideList.Length;p++)
                            {
                                if(collideList[i] == usedCollideList[p])
                                {
                                    /*Debug.Log("COMPARISON");
                                    Debug.Log(collideList[i]);
                                    Debug.Log(usedCollideList[p]);*/
                                    instRegistered = true;
                                    break;
                                } 
                            }

                            //Register Instance If Not Registered Already
                            if(!instRegistered)
                            {
                                for (int p = 0; p < usedCollideList.Length;p++)
                                {
                                    //Check For First Null Instance To Register
                                    if(usedCollideList[p] == null)
                                    {
                                        //Register Collision 
                                        usedCollideList[p] = collideList[i];

                                        //Determine Hit Direction
                                        //if (!sr.flipX) hitDir = 1;
                                        //else hitDir = -1;

                                        //Determine Hit Direction From Player Relation (EDGECASE GLITCH ON FIRST FRAME)
                                        if (trans.position.x > player.position.x) hitDir = 1;
                                        else if (trans.position.x < player.position.x) hitDir = -1;

                                        //INTERACT WITH ENEMY
                                        scr_enemy hitEnemy = collideList[i].GetComponent<scr_enemy>();
                                        hitEnemy.GetHit(hitDir); //Calls Method Built In Enemy
                                        //Debug.Log(hitEnemy);

                                        //Debug.Log("Enemy Hit From Player Attack");

                                        //Break
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }//ENEMY COLLISION CHECK

        //Write Out usedCollideList
        //WriteUsedCollisionList();

        /*
        void ResetCollideArray()
        {
            //Reset Values Of The Used Enemy Collision List 
            for (int i = 0; i < usedCollideList.Length;i++)
            {
                usedCollideList[i] = null;
            }
        }*/
    }

    public void ResetCollideArray()
    {
        //Reset Values Of The UsedEnemyCollision List
        for (int i = 0; i < usedCollideList.Length;i++)
        {
            usedCollideList[i] = null;
        }
    }

    private void WriteUsedCollisionList()
    {
        //Write Out usedCollideList
        Debug.Log("USED COLLIDE LIST");
        for (int i = 0; i < usedCollideList.Length; i++)
        {
            Debug.Log(usedCollideList[i]);
        }
    }
}
