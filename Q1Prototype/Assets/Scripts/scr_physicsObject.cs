using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_physicsObject : MonoBehaviour {

    //Component Placeholders
    Transform trans;
    BoxCollider2D bc2d;
    Rigidbody2D rb2d;
    SpriteRenderer sr;
    scr_placeMeeting pm;

    //Modifiable Variables
    public bool gravEnabled = true;

    //Physics Object Variables
    public static float minMove = .001f;
    float maxVSpeed = 2f;
    public static float grav = .015f; //.005f;
    public float vSpeed;
    public float hSpeed;

    //Define Component Vars
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        bc2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponent<scr_placeMeeting>();
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Gravity
        if (gravEnabled && !pm.PlaceMeeting(trans.position.x, trans.position.y - minMove, 0))
        {
            vSpeed -= grav;
        }

        //Cap Unreasonable VSpeed
        if (vSpeed > maxVSpeed) vSpeed = maxVSpeed;
        else if (vSpeed < -maxVSpeed) vSpeed = -maxVSpeed;

        //Vertical Collision
        if(vSpeed > 0 || vSpeed < 0)
        {
            if(!pm.PlaceMeeting(trans.position.x,trans.position.y+vSpeed,0))
            {
                trans.position += new Vector3(0,vSpeed,0);
            }
            else if(!pm.PlaceMeeting(trans.position.x,trans.position.y+minMove*pm.Sign(vSpeed),0))
            {
                do
                {
                    trans.position += new Vector3(0, minMove * pm.Sign(vSpeed), 0); //+
                }
                while (!pm.PlaceMeeting(trans.position.x, trans.position.y + minMove*pm.Sign(vSpeed), 0));
            }

            //Cease VSpeed  
            if(pm.PlaceMeeting(trans.position.x, trans.position.y + minMove * pm.Sign(vSpeed), 0))
            {
                vSpeed = 0;
            }
        }

        //Horizontal Collision
        if(hSpeed > 0 || hSpeed < 0)
        {
            if (!pm.PlaceMeeting(trans.position.x+hSpeed,trans.position.y,0))
            {
                transform.position += new Vector3(hSpeed, 0, 0);
            }
            else if(!pm.PlaceMeeting(trans.position.x+minMove*pm.Sign(hSpeed),trans.position.y,0))
            {
                do
                {
                    transform.position += new Vector3(minMove * pm.Sign(hSpeed), 0, 0);
                }
                while (!pm.PlaceMeeting(trans.position.x + minMove * pm.Sign(hSpeed), trans.position.y, 0));
            }

            //Cease HSpeed
            if (pm.PlaceMeeting(trans.position.x + minMove * pm.Sign(hSpeed), trans.position.y, 0))
            {
                hSpeed = 0;
            }
        }

        //Test Collision Color
        if (pm.PlaceMeeting(trans.position.x, trans.position.y, 0)) sr.color = new Color(255, 0, 0, sr.color.a); //Red
        else sr.color = new Color(0, 255, 0, sr.color.a); //.color = new Color(100, 255, 100, sr.color.a);       
    }
}
