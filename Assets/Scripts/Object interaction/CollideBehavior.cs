using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideBehavior : MonoBehaviour
{
    public bool karstenIsHere;
    public GameObject savedFish;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "KarstenKrabbe")
        {
            Destroy(other.gameObject); // Destroy the collided Trash object
            karstenIsHere = true;
        }
        else if (other.gameObject.name == "sixpackfish" && karstenIsHere)
        {
            karstenIsHere = false;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            savedFish.SetActive(true);
        }
    }
}


