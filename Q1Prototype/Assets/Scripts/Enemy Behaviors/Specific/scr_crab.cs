using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_crab : scr_animationManager {


    //Crab Sprite Vars
    Sprite crabSprite;

    // Use this for initialization
    void Start () {
        crabSprite = sr.sprite;
        //sprites = crabSprite.//Resources.LoadAll<Sprite>("spr_testCrab");
	}
	
	// Update is called once per frame
	void Update () {

        //Airborn 
        if (!pm.PlaceMeeting(trans.position.x, trans.position.y - minMove, 0))
        {
            //x1 = 0;
            //x2 = 3;
            //animSpeed = 5;
            //animDir = 1;
        }

        //GetComponent<SpriteRenderer>().sprite = sprites[0];
        //Debug.Log(sprites.Length);

	}
}
