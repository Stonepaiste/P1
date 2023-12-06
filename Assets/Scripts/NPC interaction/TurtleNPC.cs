using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TurtleNPC : MonoBehaviour
{
    private Camera mainCam;                          //Camera
    private PlayerMovementFisk pm;                   //spillerens script
    private Animator anim;                           //animatoren på npc
    [HideInInspector] public bool detectPlayer;       //Bool der holder styr om spilleren er tæt på npcen
    private float DialougeDelay = 0.1f;

    public GameObject deadTurtle;

    [Header("State")]
    public state currentState = state.firstmeeting;
    public enum state { firstmeeting, second, third, dead }

    [Header("dialouge boxes")]
    [SerializeField] private Vector3 textOffset;        //offset hvor tekst skal placere sig ift. npc'en selv
    [SerializeField] private GameObject firstDialouge;
    [SerializeField] private GameObject secondDialouge;
    [SerializeField] private GameObject thirdDialouge;
    [SerializeField] private GameObject deadDialouge;

    [SerializeField] private GameObject pressToTalk;      //objekt der indeholder press m tekst


    private void Start()
    {
        mainCam = Camera.main;                      //henter kamera
        anim = GetComponent<Animator>();
        pm = FindAnyObjectByType<PlayerMovementFisk>();

        //Sætter de rigtige parametre til false når spillet starter
        detectPlayer = false;
        firstDialouge.SetActive(false);
        secondDialouge.SetActive(false);
        thirdDialouge.SetActive(false);
        deadDialouge.SetActive(false);
        pressToTalk.SetActive(false);  
    }

    private void DeactivateChilden(GameObject DiffDialouge)
    {
        if (DiffDialouge != null)
        {
            foreach (Transform child in DiffDialouge.transform)
            {
                if (child != null && child.gameObject != null)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    private void Update()
    {
        //Siger til tekstbokse at de skal placere sig ved npc'ens position + det givne offset
        PlaceText();
        CheckState();
    }

    private void CheckState()
    {
        switch (GameManager.instance.currentStage)
        {
            case GameManager.gameStage.stage1:
                currentState = state.firstmeeting;
                break;

            case GameManager.gameStage.stage2:
                currentState = state.second;
                break;

            case GameManager.gameStage.stage3:
                currentState = state.third;
                break;

            case GameManager.gameStage.stage6:
                GetComponent<SpriteRenderer>().enabled = false;
                deadTurtle.SetActive(true);
                currentState = state.dead;
                break;
        } 
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
            deadDialouge.SetActive(false);
            firstDialouge.SetActive(false);
            secondDialouge.SetActive(false);
            thirdDialouge.SetActive(false);
            pressToTalk.SetActive(false);
        }
    }

    private void PlaceText()
    {
        deadDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        firstDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        secondDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        thirdDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        pressToTalk.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
    }

    public void Talk()
    {
        if (detectPlayer)
        {
            pressToTalk.SetActive(false);
            pm.canMove = false;
            pm.canTalk = false;

            DeactivateChilden(firstDialouge);  
            DeactivateChilden(secondDialouge);  
            DeactivateChilden(thirdDialouge);
            
            switch (currentState)
            {
                case state.firstmeeting:
                    StartCoroutine(ActivateDialogueWithDelay(firstDialouge));
                    //firstDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    break;

                case state.second:
                    StartCoroutine(ActivateDialogueWithDelay(secondDialouge));
                    //secondDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    GameManager.instance.currentStage = GameManager.gameStage.stage3;
                    GameManager.instance.DeactivateObjectsForStage(GameManager.instance.currentCoralStage);
                    GameManager.instance.currentCoralStage++;
                    GameManager.instance.ActivateObjectsForStage(GameManager.instance.currentCoralStage);
                    break;

                case state.third:
                    StartCoroutine(ActivateDialogueWithDelay(thirdDialouge));
                    //thirdDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    //DØD
                    break;

                case state.dead:
                    StartCoroutine(ActivateDialogueWithDelay(deadDialouge));
                    bool videoStart = true;
                    if(videoStart == true)
                    {
                        GameManager.instance.StartVideo();
                        videoStart = false;
                        pm.canMove = false;
                        
                    }
                    break;
            }

        }
    }

    private IEnumerator ActivateDialogueWithDelay(GameObject Dialogue)
    {
        if (Dialogue != null)
        {
            Dialogue.SetActive(true);

            foreach (Transform child in Dialogue.transform)
            {
                if (child != null && child.gameObject != null)
                {
                    child.gameObject.SetActive(true);
                    yield return new WaitForSeconds(DialougeDelay);
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.M));
                    yield return new WaitForSeconds(DialougeDelay);
                    child.gameObject.SetActive(false);
                }
            }

            Dialogue.SetActive(false);
            pm.canMove = true;
            pm.canTalk = true;
        }
            
    }

}
