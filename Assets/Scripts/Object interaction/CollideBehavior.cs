using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideBehavior : MonoBehaviour
{
    public bool HasBottleTrash;
    public bool HasOilBarrelTrash;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Angel")
        {
            Debug.Log("Trash collision");
            Destroy(other.gameObject); // Destroy the collided Trash object
            HasBottleTrash = true;

        }
        else if (other.gameObject.name == "Clow" && HasBottleTrash)
        {
            Debug.Log("Quest collision");
            HasBottleTrash = false;

        }
    }
}


