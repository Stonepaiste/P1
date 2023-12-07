using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CodNPC : MonoBehaviour
{
    private Camera mainCam;                          //Camera
    [SerializeField] PlayerMovementFisk pm;                   //spillerens script
    private Animator anim;                           //animatoren på npc
    [HideInInspector]public bool detectPlayer;       //Bool der holder styr om spilleren er tæt på npcen

    [SerializeField] private float waitToMoveTime = 5;

    private float DialougeDelay = 0.1f;

        
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

        foreach (Transform child in firstDialouge.transform)
        {
            child.gameObject.SetActive(false);
        }
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
            DeactivateDialogue();
        }
    }

    private void PlaceText()
    {
        firstDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        pressToTalk.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
    }

    private void DeactivateDialogue()
    {
        firstDialouge.SetActive(false);
        pressToTalk.SetActive(false);
    }

    public void Talk()
    {
        if (detectPlayer)
        {
            pressToTalk.SetActive(false);
            pm.canMove = false;
            pm.canTalk = false;

            if (currentState != state.dead)
            {
                StartCoroutine(DialogueAndMove());
                StartCoroutine(WaitToMove(waitToMoveTime));
            }
        }
    }

    private IEnumerator DialogueAndMove()
    {
        yield return StartCoroutine(ActivateDialogueWithDelay());
    }

    private IEnumerator ActivateDialogueWithDelay()
    {
        
        firstDialouge.SetActive(true);

        foreach (Transform child in firstDialouge.transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(DialougeDelay);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.M));
            yield return new WaitForSeconds(DialougeDelay);
            child.gameObject.SetActive(false);
        }
        pm.canMove = true;
        pm.canTalk = true;
        DeactivateDialogue();
        GameManager.instance.currentStage = GameManager.gameStage.stage2;
        GameManager.instance.IncreaseCoralStage();
        anim.SetTrigger("dead");
    }
    
    private IEnumerator WaitToMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        currentState = state.dead;
      
        
        pm.canMove = true;
        pm.canTalk = true;
    }
}
