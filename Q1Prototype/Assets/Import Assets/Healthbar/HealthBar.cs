using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    void Start()
    {
    }

    public void SetSize(float sizeNormalized)
    {
        Transform b = transform.Find("Bar");
        b.localScale = new Vector3(sizeNormalized, 1f);
    }
}
