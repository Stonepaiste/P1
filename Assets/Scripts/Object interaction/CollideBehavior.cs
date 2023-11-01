using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//hello sexy
public class CollideBehavior : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
 

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Trash")
        {
            Debug.Log("Ramt");
            ChangeSprite();

        }
    }

    void ChangeSprite()
    {
        spriteRenderer.sprite = newSprite;
    }

}


