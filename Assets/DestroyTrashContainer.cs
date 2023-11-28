using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrashContainer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trash"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
