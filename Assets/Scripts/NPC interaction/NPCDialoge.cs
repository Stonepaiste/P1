using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialoge : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField]private GameObject dialogeBox;
    [SerializeField]private Vector3 boxOffset;
    [HideInInspector]public bool detectPlayer;

    private void Start()
    {
        mainCam = Camera.main;
        dialogeBox.transform.position = mainCam.WorldToScreenPoint(transform.position + boxOffset);

        detectPlayer = false;
        dialogeBox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = true;
            dialogeBox.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = false;
            dialogeBox.SetActive(false);
        }
    }

    public void Talk()
    {
        if (detectPlayer)
        {
            dialogeBox.SetActive(true);
        }
    }

}
