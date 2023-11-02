using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialoge : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField]private Animator anim;
    [SerializeField]private GameObject pressM;
    [SerializeField]private GameObject dialogeBox;
    [SerializeField]private Vector3 textOffset;
    [HideInInspector]public bool detectPlayer;


    private void Start()
    {
        mainCam = Camera.main;
        dialogeBox.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        pressM.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);

        detectPlayer = false;
        dialogeBox.SetActive(false);
        pressM.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = true;
            pressM.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = false;
            dialogeBox.SetActive(false);
            pressM.SetActive(false);
        }
    }

    public void Talk()
    {
        if (detectPlayer)
        {
            pressM.SetActive(false);
            anim.SetTrigger("Animate");
            dialogeBox.SetActive(true);
        }
    }
}
