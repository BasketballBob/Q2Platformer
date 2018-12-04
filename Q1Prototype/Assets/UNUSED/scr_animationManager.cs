
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_animationManager : MonoBehaviour {

    //Component References
    protected Transform trans;
    protected SpriteRenderer sr;
    protected scr_placeMeeting pm;
    protected float minMove = scr_physicsObject.minMove;

    //Animation Manager Vars
    protected int animSpeed = 2;
    int animAlarm;
    protected int animDir = 1; 
    protected int spriteX; //Current Sprite
    protected int x1; //Start Of Animation
    protected int x2; //End Of Animation

    //Inserted Sprite List
    protected Sprite[] sprites;

    //Define Reference Vars
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

        //Clamp Sprite 
        if (spriteX < x1) spriteX = x1;
        else if (spriteX > x2) spriteX = x2;

        //Countdown Animation Alarm
        if (animAlarm > 0) animAlarm -= 1;

        //Play Animation
        else
        {
            //Advance SpriteX Pos
            spriteX += animDir;

            //Loop If Necessary (Not same as clamp)
            if (spriteX < x1) spriteX = x2;
            else if (spriteX > x2) spriteX = x1;

            //Reset Animation Alarm
            animAlarm = animSpeed;
        }

        //Set Sprite Value
        sr.sprite = sprites[spriteX];



    }
}
