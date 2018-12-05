using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_cameraBehavior : MonoBehaviour {

    //Component Reference Variables
    public Transform followedObject;
    Transform trans;

    //Camera Variables
    public static float width = 13;
    public static float height = 10;
    public float x1 = -3;
    public float x2 = 10;

    //Toggled Features
    bool bgEnabled = true;
    bool healthEnabled = false;

    //Background Variables
    public Sprite bgImage;
    GameObject bgObject;
    public Color bgColor;
    float bgScale = 6f;
    int bgLayer = -2;

    //Healthbar Variables
    public Sprite healthImage;
    GameObject healthObject;
    scr_player player;
    float healthOffX = -4f; //Offset of Camera (0,0)
    float healthOffY = -4f;
    int healthLayer = 2;
    

    //Component Variable Initialization
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        player = GameObject.Find("obj_player").GetComponent<scr_player>();
    }

    //Create Background
    void Start()
    {
        //Create Background Object
        bgObject = new GameObject();
        bgObject.AddComponent<SpriteRenderer>();

        //Modify Background Object
        bgObject.GetComponent<SpriteRenderer>().sprite = bgImage;
        bgObject.GetComponent<Transform>().localScale = new Vector3(bgScale, bgScale, bgScale);
        bgObject.GetComponent<SpriteRenderer>().sortingOrder = bgLayer;
        bgObject.GetComponent<SpriteRenderer>().color = bgColor;

        //Create Healthbar
        healthObject = new GameObject();
        healthObject.AddComponent<SpriteRenderer>();
        healthObject.GetComponent<SpriteRenderer>().sprite = healthImage;
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

        //Background 
        if (bgEnabled)
        {
            //Manage Background
            float bgWidth = bgObject.GetComponent<SpriteRenderer>().bounds.size.x;
            float bgWidthPortion = .48f; //(Edgecase Variable For When Edge Of Background is Visible)
            float bgX1 = x1 + Mathf.Abs(bgWidth * bgWidthPortion - width / 2); //bgWidth /2 
            float bgX2 = x2 - Mathf.Abs(bgWidth * bgWidthPortion - width / 2); //bgWidth / 2
            float backgroundX = bgX1 + (bgX2 - bgX1) * ((trans.position.x - x1) / (x2 - x1));
            bgObject.GetComponent<Transform>().position = new Vector2(backgroundX, trans.position.y);
        }
        else bgObject.SetActive(false);

        //Player Health Bar
        if(healthEnabled)
        {
            //Health Bar Position
            //healthObject.GetComponent<Transform>.position = new Vector2 
        }
    }
}
