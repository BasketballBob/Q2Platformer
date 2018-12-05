using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_enemy : MonoBehaviour {

    //Component Reference Variables
    Transform trans;
    SpriteRenderer sr;
    scr_physicsObject po;
    scr_placeMeeting pm;

    //Enemy Variables
    float minMove = scr_physicsObject.minMove;
    int drawLayer = 0;
    int hDir;

    //Modifiable Enemy Variables
    public bool dead = false;
    [SerializeField] bool knockable;
    [SerializeField] int hp = 1;
    float vKnockback = .125f;
    float hKnockback = .065f;
    bool stunnable = true;
    public bool stunned = false;
    bool hKnockStop = false;
    float flickerAlarm = 0;
    float flickerTime = 30;
    float flickerDir = 1;
    float flickerRate = .25f; //.15f
    float flickerMin = .4f;

    //Define All Component References
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        po = GetComponent<scr_physicsObject>();
        pm = GetComponent<scr_placeMeeting>();
    }

    // Use this for initialization
    void Start () {
        sr.sortingOrder = drawLayer;
	}
	
	// Update is called once per frame
	void Update () {

        //Stop Horizontal Knockback Sliding
        if(hKnockStop && pm.PlaceMeeting(trans.position.x,trans.position.y-minMove,0) && po.vSpeed <= 0)
        {
            //Debug.Log("UnhKnockStop");
            po.hSpeed = 0;
            hKnockStop = false;
        }

        //Unstun Enemy
        if(stunned && pm.PlaceMeeting(trans.position.x,trans.position.y-minMove,0) && po.vSpeed <= 0 && !dead)
        {
            stunned = false;
        }

        //Enemy AI (ADDED WITHIN CUSTOM COMPONENT)

        //Manage Opacity From Flickering
        if (flickerAlarm > 0 && !dead)
        {
            //SetAlpha(1f - flickerMin * (flickerAlarm / flickerTime));

            //Manual 2-Stage Flickering Effect
            //if (sr.color.a > .5) sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            //else sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);

            //One-way Subtractive Flickering
            //sr.color -= new Color(0, 0, 0, flickerRate);
            //if (sr.color.a < 0) SetAlpha(1);

            //Two-way Bouncing Flickering
            if (sr.color.a + flickerDir * flickerRate > 1)
            {
                SetAlpha(1);
                flickerDir *= -1;
            }
            else if (sr.color.a + flickerDir * flickerRate < flickerMin)
            {
                SetAlpha(flickerMin);
                flickerDir *= -1;
            }
            else SetAlpha(sr.color.a + flickerDir * flickerRate);    

            flickerAlarm -= 1;
        }
        //Manage Opacity On Death
        else if(dead)
        {
            SetAlpha(.5f);
        }
        else SetAlpha(1f);

        //Die Once Out Of HP
        if(hp <= 0 && !dead)
        {
            dead = true;
            //Destroy(GetComponent<scr_enemy>());
            //Destroy(this.gameObject, 0.0f);
        }
	}

    public void GetHit(float hitDir)
    {
        //Debug.Log("Enemy Hit");

        //Stun Enemy
        hKnockStop = true;
        if (stunnable) stunned = true;
           
        //Knockback Enemy
        if (knockable)
        {
            po.vSpeed = vKnockback;
            po.hSpeed = hKnockback * hitDir;
        }

        //Make Enemy Flicker
        flickerAlarm = flickerTime;

        //Take Damage 
        hp -= 1;
    }

    private void SetAlpha(float alpha)
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }
}
