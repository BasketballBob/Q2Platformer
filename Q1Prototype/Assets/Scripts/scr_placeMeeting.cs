using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_placeMeeting : MonoBehaviour {

    //Component Placeholders
    Transform trans;
    BoxCollider2D bc2d;
    ContactFilter2D contactFilter;
    Collider2D[] collideResults = new Collider2D[16];

    //Define Component Variables
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool PlaceMeeting(float x, float y, int collisionType)
    {
        //Move Transform To New Position
        Vector3 prevPos = trans.position;
        trans.position = new Vector3(x, y, trans.position.z);
        bool returnVal = false;

        //Check For Collision 
        if (Physics2D.OverlapCollider(bc2d, contactFilter, collideResults) > 0)
        {
            //Debug.Log("COLLISION FOUND"); 

            //Check All Instances
            for(int i = 0;i < collideResults.Length;i++)
            {
                //Check If Colliding Instance Exists
                if(collideResults[i] != null)
                {
                    //Check For Collision With Component
                    //0 - Wall / Platform
                    //1 - Player
                    //2 - Enemy
                    //3 - Player Attack
                    //4 - Enemy Attack
                    //5 - Player Block

                    //Set Colliding Object To Variables
                    GameObject collidingObject = collideResults[i].gameObject;

                    //Wall
                    if (collisionType == 0 && collidingObject.GetComponent<scr_wall>() != null)
                    //|| collisionType == 0 && collidingObject.GetComponent<>)
                    {
                        //Debug.Log("WALL");
                        returnVal = true;
                        break;
                    }
                    //Player
                    else if (collisionType == 1 && collidingObject.GetComponent<scr_player>() != null)
                    {
                        returnVal = true;
                        break;
                    }
                    //Enemy
                    else if (collisionType == 2 && collidingObject.GetComponent<scr_enemy>() != null
                    && !collideResults[i].GetComponent<scr_enemy>().dead)
                    { //ONLY COLLIDE WITH ENEMY IF ALIVE
                        returnVal = true;
                        break;
                    }
                    //Player Attack
                    else if (collisionType == 3 && collidingObject.GetComponent<scr_playerAttack>() != null)
                    {
                        returnVal = true;
                        break;
                    }
                    //Enemy Attack 
                    else if (collisionType == 4 && collidingObject.GetComponent<scr_enemyAttack>() != null)
                    {
                        returnVal = true;
                        break;
                    }
                    //Player Block 
                    else if (collisionType == 5 && collidingObject.GetComponent<scr_playerBlock>() != null)
                    {
                        returnVal = true;
                        break;
                    }

                    //Platform
                    else if(collisionType == 0 && collidingObject.GetComponent<scr_platform>() != null
                    && gameObject.GetComponent<Transform>().position.y-gameObject.GetComponent<BoxCollider2D>().size.y/2 
                    > collidingObject.GetComponent<Transform>().position.y+collidingObject.GetComponent<BoxCollider2D>().size.y/2)
                    { //COLLISION ONLY WORKS IF THE SPRITE PIVOT IS PERFECTLY CENTERED
                        returnVal = true;
                        break;
                    }
                }
            }

            //Print Collide Results
            //(CURRENTLY LAGS THE GAME)
            /*Debug.Log("COLLISION RESULTS:");
            for (int i = 0; i < collideResults.Length; i++)
            {
                if (collideResults[i] != null) Debug.Log(collideResults[i]);
            }*/

            //Reset Collision Array (EMPTY ARRAY FOR NEXT COLLISION CHECK)
            for (int i = 0; i < collideResults.Length; i++)
            {
                collideResults[i] = null;
            }
        }

        //Return Transform To Intial Position
        trans.position = prevPos;

        //Return ReturnVal
        return returnVal;
    }

    public float Sign(float modFloat)
    {
        //Determine Sign
        if (modFloat > 0) return 1;
        else if (modFloat < 0) return -1;
        else return 0;
    }
}
