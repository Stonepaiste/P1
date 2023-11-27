using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CodNPC : MonoBehaviour
{
    private Camera mainCam;                          //Camera
    private PlayerMovementFisk pm;                   //spillerens script
    private Animator anim;                           //animatoren på npc
    [HideInInspector]public bool detectPlayer;       //Bool der holder styr om spilleren er tæt på npcen

    [SerializeField] private float waitToMoveTime = 5;
        
    [Header("State")]
    public state currentState = state.firstmeeting;
    public enum state { firstmeeting, dead }

    [Header("dialouge boxes")]
    [SerializeField] private Vector3 textOffset;     //offset hvor tekst skal placere sig ift. npc'en selv
    [SerializeField]private GameObject firstDialouge;
    [SerializeField]private GameObject pressToTalk;      //objekt der indeholder press m tekst


    private void Start()
    {
        mainCam = Camera.main;                      //henter kamera
        anim = GetComponent<Animator>();
        pm = FindAnyObjectByType<PlayerMovementFisk>();

        //Sætter de rigtige parametre til false når spillet starter
        detectPlayer = false;
        firstDialouge.SetActive(false);
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
            pressToTalk.SetActive(false);
        }
    }

    private void PlaceText()
    {
        firstDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        pressToTalk.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
    }

    public void Talk()
    {
        if (detectPlayer)
        {
            pressToTalk.SetActive(false);

            if (currentState != state.dead)
            {
                StartCoroutine(WaitToMove(waitToMoveTime));
                pm.canMove = false;
                pm.canTalk = false;
            }

            switch (currentState)
            {
                case state.firstmeeting:
                    firstDialouge.SetActive(true);
                    firstDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    break;

                case state.dead:
                    anim.SetTrigger("dead");
                    break;
            }
        }
    }

    private IEnumerator WaitToMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        currentState = state.dead;
        GameManager.instance.currentStage = GameManager.gameStage.stage2;
        GameManager.instance.DeactivateObjectsForStage(GameManager.instance.currentCoralStage);
        GameManager.instance.currentCoralStage++;
        GameManager.instance.ActivateObjectsForStage(GameManager.instance.currentCoralStage);
        
        pm.canMove = true;
        pm.canTalk = true;
    }
}
