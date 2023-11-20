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
        if (other.gameObject.name == "Flaske_Prop")
        {
            Debug.Log("Trash collision");
            Destroy(other.gameObject); // Destroy the collided Trash object
            HasBottleTrash = true;

        }
        else if (other.gameObject.name == "Quest1" && HasBottleTrash)

        {
            Debug.Log("Quest collision");
            HasBottleTrash = false;

        }


        if (other.gameObject.name == "OilBarrel_Prop")
        {
            Debug.Log("Trash collision");
            Destroy(other.gameObject); // Destroy the collided Trash object
            HasOilBarrelTrash = true;

        }
        else if (other.gameObject.name == "Quest2" && HasBottleTrash)

        {
            Debug.Log("Quest collision");
            HasOilBarrelTrash = false;

        }

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


