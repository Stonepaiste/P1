using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialoge : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField]private GameObject dialogeBox;
    [SerializeField]private Vector3 boxOffset;

    private void Start()
    {
        mainCam = Camera.main;
        dialogeBox.transform.position = mainCam.WorldToScreenPoint(transform.position + boxOffset);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("her");

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ikke her");
        }
    }


}
