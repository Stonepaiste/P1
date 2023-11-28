using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrashContainer : MonoBehaviour
{
    public int trashcollected = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trash"))
        {
            trashcollected++;
            Destroy(other.gameObject);
        }
    }
}
