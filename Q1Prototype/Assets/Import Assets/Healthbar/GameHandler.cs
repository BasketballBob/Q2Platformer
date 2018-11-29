using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    //Player Reference Variables
    scr_player player;

    private HealthBar healthBar; //[SerializeField] 
    static float sz = 0f;
    public float szChange = 0.01f;

    //Define Reference Variables
    private void OnEnable()
    {
        healthBar = GetComponent<HealthBar>();
        player = GameObject.Find("obj_player").GetComponent<scr_player>();
    }

    // Use this for initialization
    private void Start()
    {

        healthBar.SetSize(1.0f);
    }

    private void Update()
    {
        //sz = sz + szChange;
        /*if (sz > 1)
        {
            szChange = -szChange;
        }
        if (sz < 0)
        {
            szChange = -szChange;
        }*/

        //Get Player Health Scale
        float playerScale = (float)(player.health / player.healthCap);

        //Set Health Bar Scale
        healthBar.SetSize(sz);
        
    }
}
