using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NPCDialoge : MonoBehaviour
{
    private Camera mainCam;                          //Camera
    [SerializeField] private Vector3 textOffset;     //offset hvor tekst skal placere sig ift. npc'en selv

    public enum state { firstmeeting, help, thankyou, dead, follow }
    public state currentState = state.firstmeeting;

    [SerializeField] private float waitToMoveSoren = 5;
    [SerializeField] private float moveInstant = 0;

    [Header("Dialouge Textboxes")]
    //objekter der indeholder dialogtekst
    [SerializeField]private GameObject firstDialouge;
    [SerializeField]private GameObject helpDialouge;
    [SerializeField]private GameObject thankyouDialouge;
    [SerializeField]private GameObject pressToTalk;      //objekt der indeholder press m tekst
    private Animator anim;

    [HideInInspector]public bool detectPlayer;       //Bool der holder styr om spilleren er tæt på npcen

    private void Start()
    {
        mainCam = Camera.main;                      //henter kamera
        anim = GetComponent<Animator>();

        //Sætter de rigtige parametre til false når spillet starter
        detectPlayer = false;
        firstDialouge.SetActive(false);
        helpDialouge.SetActive(false);
        thankyouDialouge.SetActive(false);
        pressToTalk.SetActive(false);
    }

    private void Update()
    {
        //Siger til tekstbokse at de skal placere sig ved npc'ens position + det givne offset
        PlaceText();
    }

    private void OnTriggerEnter2D(Collider2D other)
    //Hvis spilleren kommer ind i collideren bliver bool sat til true og press m tekst aktiveret
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = true;
            pressToTalk.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    //Hvis spilleren går ud a collideren slukkes der for alt tekst igen
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = false;
            firstDialouge.SetActive(false);
            helpDialouge.SetActive(false);
            thankyouDialouge.SetActive(false);
            pressToTalk.SetActive(false);
        }
    }

    private void PlaceText()
    {
        firstDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        helpDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        thankyouDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        pressToTalk.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
    }

    public void Talk()
    {
        if (detectPlayer)
        {
            pressToTalk.SetActive(false);

            if(currentState == NPCDialoge.state.firstmeeting)
                StartCoroutine(WaitToMove(waitToMoveSoren));


            switch (currentState)
            {
                case state.firstmeeting:
                    firstDialouge.SetActive(true);
                    firstDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    currentState = state.help;
                    break;

                case state.help:
                    if(firstDialouge.activeInHierarchy)
                        firstDialouge.SetActive(false);

                    helpDialouge.SetActive(true);
                    helpDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    break;

                case state.thankyou:
                    thankyouDialouge.SetActive(true);
                    thankyouDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    break;

                case state.dead:
                    anim.SetTrigger("dead");

                    break;

                case state.follow:
                    //follow logic
                    break;
            }
        }
    }

    private IEnumerator WaitToMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PlayerMovementFisk pm = GameObject.FindAnyObjectByType<PlayerMovementFisk>();
        pm.canMove = true;
        pm.canTalk = true;

    }

}
