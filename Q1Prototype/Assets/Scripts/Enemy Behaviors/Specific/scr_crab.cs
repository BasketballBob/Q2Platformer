using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_crab : MonoBehaviour {

    //Component Reference Variables
    Transform trans;
    Animator anim;
    scr_placeMeeting pm;
    scr_physicsObject po;
    scr_enemy enemy;

    //Crab Sprite Vars
    float minMove = scr_physicsObject.minMove;
    Sprite crabSprite;

    //Define Component References 
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        pm = GetComponent<scr_placeMeeting>();
        po = GetComponent<scr_physicsObject>();
        enemy = GetComponent<scr_enemy>();
    }

    // Use this for initialization
    void Start () {
        //crabSprite = sr.sprite;
        //sprites = crabSprite.//Resources.LoadAll<Sprite>("spr_testCrab");
	}
	
	// Update is called once per frame
	void Update () {

        //Dead 
        if (enemy.dead)
        {
            anim.Play("anim_crab_idle");
        }
        //Airborn 
        else if (!pm.PlaceMeeting(trans.position.x, trans.position.y - minMove, 0))
        {
            anim.Play("anim_crab_airborn");
        }
        //Walking 
        else if (po.xPrev != trans.position.x)
        {
            anim.Play("anim_crab_walking");
        }
        //Idle 
        else
        {
            anim.Play("anim_crab_idle");
        }

        //GetComponent<SpriteRenderer>().sprite = sprites[0];
        //Debug.Log(sprites.Length);

	}
}
