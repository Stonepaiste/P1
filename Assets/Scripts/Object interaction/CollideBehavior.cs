using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideBehavior : MonoBehaviour
{
    public bool karstenIsHere;
    public bool targetIsSixpackfish = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "KarstenKrabbe")
        {
            karstenIsHere = true;
        }
        else if (other.gameObject.name == "sixpackfish" && karstenIsHere)
        {
            karstenIsHere = false;
            targetIsSixpackfish = true;
        }
    }
}


