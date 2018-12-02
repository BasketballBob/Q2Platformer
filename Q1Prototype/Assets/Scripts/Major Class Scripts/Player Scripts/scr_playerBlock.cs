using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_playerBlock : MonoBehaviour {

    //Component Variables
    Transform trans;
    SpriteRenderer sr;
    scr_placeMeeting pm;

    //Player Block Variables
    float hDir;
    public bool blocked = false;

    //Define Player Component Vars
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponent<scr_placeMeeting>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Determine Block Direction From Player Relation (FINISH LATER)

        //Determine If An Attack Has Been Blocked (NOW DETERMINED THROUGH ENEMY ATTACK)
        /*if (pm.PlaceMeeting(trans.position.x, trans.position.y, 4) && !blocked)
        {
            blocked = true;
        }*/
	}
}
