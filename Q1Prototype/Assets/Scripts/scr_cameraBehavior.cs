using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_cameraBehavior : MonoBehaviour {

    //Component Reference Variables
    public Transform followedObject;
    Transform trans;

    //Camera Variables
    public static float width = 16;
    public static float height = 10;
    public float x1 = -3;
    public float x2 = 10;

    //Component Variable Initialization
    private void OnEnable()
    {
        trans = GetComponent<Transform>(); 
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //trans.position += new Vector3(.01f, 0, 0);


    }

    //Follow Followed Object
    private void LateUpdate()
    {
        //Track Horizontally
        trans.position = new Vector3(followedObject.position.x, trans.position.y, trans.position.z);
        //Debug.Log(followedObject.position.x);

        //Clamp Horizontally
        if (followedObject.position.x < x1)
        {
            trans.position = new Vector3(x1, trans.position.y, trans.position.z);
        }
        else if (followedObject.position.x > x2)
        {
            trans.position = new Vector3(x2, trans.position.y, trans.position.z);
        }
    }
}
