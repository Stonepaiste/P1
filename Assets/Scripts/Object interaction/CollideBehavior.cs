using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideBehavior : MonoBehaviour
{

    private Animator myAnimator; // We need this in order to update our animations with our animator
    public bool HasTrash1;

    public GameObject trashcanvas;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        trashcanvas = GameObject.Find("TrashCollected");
        trashcanvas.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name =="Trash")
        {
            Debug.Log("Trash collision");
            myAnimator.SetBool("HasTrash", true);
            Destroy(other.gameObject); // Destroy the collided Trash object
            HasTrash1 = true;
            trashcanvas.SetActive(true);

        }
        else if (other.gameObject.name == "Quest" && HasTrash1)

        {
            Debug.Log("Quest collision");
            myAnimator.SetBool("HasTrash", false);
            HasTrash1 = false;
            trashcanvas.SetActive(false);

        }
        else
        {
            myAnimator.SetBool("HasTrash", false);
        }
    }

    //Din Far
}


