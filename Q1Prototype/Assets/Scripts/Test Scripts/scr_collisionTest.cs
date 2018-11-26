using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_collisionTest : MonoBehaviour {

    //Component References 
	SpriteRenderer sr;
	BoxCollider2D bc2d;
	ContactFilter2D contactFilter; 
	Collider2D[] collideResults = new Collider2D[16];
    
    //Define Component References
	private void OnEnable()
    {
		sr = GetComponent<SpriteRenderer>();
		bc2d = GetComponent<BoxCollider2D>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		 //Test Collision Color
        if (Physics2D.OverlapCollider(bc2d, contactFilter, collideResults) > 0) sr.color = new Color(255, 0, 0, sr.color.a); //Red
        else sr.color = new Color(0, 255, 0, sr.color.a); //.color = new Color(100, 255, 100, sr.color.a);     
	}
}
