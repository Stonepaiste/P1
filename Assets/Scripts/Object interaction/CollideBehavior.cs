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
        if (other.gameObject.name == "KarstenKrabbe" && GameManager.instance.currentStage == GameManager.gameStage.stage4)
        {
            karstenIsHere = true;
        }
        else if (other.gameObject.name == "sixpackfish" && karstenIsHere == true)
        {
            karstenIsHere = false;
            targetIsSixpackfish = true;
        }
    }
}


