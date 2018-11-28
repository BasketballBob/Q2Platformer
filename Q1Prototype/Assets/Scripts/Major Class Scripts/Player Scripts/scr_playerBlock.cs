using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_playerBlock : MonoBehaviour {

    //Component Variables
    Transform trans;
    SpriteRenderer sr;

    //Player Block Variables
    float hDir;

    //Define Player Component Vars
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        //Determine Block Direction From Player Relation (FINISH LATER)
	}
}
