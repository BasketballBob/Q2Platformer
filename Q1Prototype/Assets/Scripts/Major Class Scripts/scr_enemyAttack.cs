using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_enemyAttack : MonoBehaviour {

    //Reference Variables
    Transform trans;
    scr_placeMeeting pm;
    scr_player player;

    //Enemy Attack Variables
    bool damaging = true;
    int hDir = 1;


    //Initialize Reference Vars
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        pm = GetComponent<scr_placeMeeting>();
        player = GameObject.Find("obj_player").GetComponent<scr_player>();
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(player.hDir);
        Debug.Log(hDir);
        //Pacify If Blocked (BLOCKING DIRECTION DETERMINED BY PLAYER DIRECTION NOT BLOCK DIRECTION)
        if(damaging && pm.PlaceMeeting(trans.position.x,trans.position.y,5) && player.hDir == -hDir)
        {
            damaging = false;
        }

		//Damage If Damaging
        else if(damaging && pm.PlaceMeeting(trans.position.x,trans.position.y,1))
        {
            //Debug.Log("Damage Outputting");

            //Damage Player 
            player.takeDamage();

            //Pacify Attack
            damaging = false;
        }
	}

    public void ReactivateDamage()
    {
        damaging = true;
    }
}
