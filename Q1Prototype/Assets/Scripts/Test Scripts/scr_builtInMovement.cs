using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_builtInMovement : MonoBehaviour {

    //Component References
    Rigidbody2D rb2d;

    //Test Player
    public float jumpSpeed = 300f;
    public float moveSpeed = 20f;

    //Define Component References
    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Detect Player Input 
        bool jump = Input.GetKeyDown(KeyCode.W);
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);

        //Jump
        if(jump)
        {
            rb2d.AddForce(new Vector2(0,jumpSpeed));
        }

        //Move Horizontally
        if(right)
        {
            rb2d.AddForce(new Vector2(moveSpeed, 0));
        }
        else if(left)
        {
            rb2d.AddForce(new Vector2(-moveSpeed, 0));
        }
	}
}
