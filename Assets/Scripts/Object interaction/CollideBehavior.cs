using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideBehavior : MonoBehaviour
{

    public bool HasBottleTrash;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Flaske_Prop")
        {
            Debug.Log("Trash collision");
            Destroy(other.gameObject); // Destroy the collided Trash object
            HasBottleTrash = true;

        }
        else if (other.gameObject.name == "Quest" && HasBottleTrash)

        {
            Debug.Log("Quest collision");
            HasBottleTrash = false;

        }
    }

}


