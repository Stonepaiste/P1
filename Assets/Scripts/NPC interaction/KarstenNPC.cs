using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class KarstenNPC : MonoBehaviour
{
    private Camera mainCam;                          //Camera
    private PlayerMovementFisk pm;                   //spillerens script
    private CollideBehavior cb;
    private SixpackFish sixpackFish;
    [HideInInspector] public bool detectPlayer;       //Bool der holder styr om spilleren er tæt på npcen
    private Animator anim;
    public GameObject savedFish;


    [SerializeField] private float waitToMoveTime = 5;
    public int trashNeeded = 10;
    public DestroyTrashContainer container;
    public bool followPlayer;
    public float speed = 4;
    public float distanceToPlayer = 2;

    [Header("State")]
    public state currentState = state.firstmeeting;
    public enum state { early, firstmeeting, follow, finished }

    [Header("dialouge boxes")]
    [SerializeField] private Vector3 textOffset;        //offset hvor tekst skal placere sig ift. npc'en selv
    [SerializeField] private GameObject helpWithTrash;
    [SerializeField] private GameObject helpWithTrashStill;
    [SerializeField] private GameObject early;
    [SerializeField] private GameObject thankyouDialouge;
    [SerializeField] private GameObject finishedDialouge;
    [SerializeField] private GameObject pressToTalk;      //objekt der indeholder press m tekst


    private void Start()
    {
        mainCam = Camera.main;                      //henter kamera
        pm = FindAnyObjectByType<PlayerMovementFisk>();
        cb = FindAnyObjectByType<CollideBehavior>();
        anim = GetComponent<Animator>();
        sixpackFish = FindAnyObjectByType<SixpackFish>();

        //Sætter de rigtige parametre til false når spillet starter
        detectPlayer = false;
        helpWithTrash.SetActive(false);
        early.SetActive(false);
        finishedDialouge.SetActive(false);
        pressToTalk.SetActive(false);
    }

    private void Update()
    {
        //Siger til tekstbokse at de skal placere sig ved npc'ens position + det givne offset
        PlaceText();
        CheckState();
        Follow();
    }

    private void CheckState()
    {
        switch (GameManager.instance.currentStage)
        {
            case GameManager.gameStage.stage1:
                currentState = state.early;
                break;

            case GameManager.gameStage.stage2:
                currentState = state.early;
                break;

            case GameManager.gameStage.stage3:
                currentState = state.early;
                break;

            case GameManager.gameStage.stage4:
                currentState = state.firstmeeting;
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
        if (other.CompareTag("Sixpackfish"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            savedFish.SetActive(true);
            sixpackFish.currentState = SixpackFish.state.saved;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    //Hvis spilleren går ud a collideren slukkes der for alt tekst igen
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = false;
            helpWithTrash.SetActive(false);
            helpWithTrashStill.SetActive(false);
            early.SetActive(false);
            finishedDialouge.SetActive(false);
            pressToTalk.SetActive(false);
            thankyouDialouge.SetActive(false);

        }
    }

    private void PlaceText()
    {
        helpWithTrash.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        early.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        finishedDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        pressToTalk.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        thankyouDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        helpWithTrashStill.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
    }

    public void Talk()
    {
        if (detectPlayer)
        {
            pressToTalk.SetActive(false);

            StartCoroutine(WaitToMove(waitToMoveTime));
            pm.canMove = false;
            pm.canTalk = false;

            if (container.trashcollected >= trashNeeded)
                followPlayer = true;

            switch (currentState)
            {
                case state.early:
                    early.SetActive(true);
                    early.GetComponent<Animator>().SetTrigger("Animate");
                    break;

                case state.firstmeeting:
                    //Sige hjælp med skrald
                    helpWithTrash.SetActive(true);
                    helpWithTrash.GetComponent<Animator>().SetTrigger("Animate");
                    GameManager.instance.DeactivateObjectsForStage(GameManager.instance.currentCoralStage);
                    GameManager.instance.currentCoralStage++;
                    GameManager.instance.ActivateObjectsForStage(GameManager.instance.currentCoralStage);
                    GameManager.instance.currentStage = GameManager.gameStage.stage5;
                    currentState = state.follow;
                    break;

                case state.follow:
                    if(container.trashcollected >= trashNeeded)
                    {
                        thankyouDialouge.SetActive(true);
                        thankyouDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    }
                    else
                    {
                        helpWithTrashStill.SetActive(true);
                        helpWithTrashStill.GetComponent<Animator>().SetTrigger("Animate");
                    }
                    break;

                case state.finished:
                    early.SetActive(false);
                    finishedDialouge.SetActive(true);
                    finishedDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    //DØD
                    break;
            }
        }
    }

    private IEnumerator WaitToMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (GameManager.instance.currentStage == GameManager.gameStage.stage4)
            GameManager.instance.currentStage = GameManager.gameStage.stage5;

        pm.canMove = true;
        pm.canTalk = true;
    }

    private void Follow()
    {
        var crabSpeed = speed * Time.deltaTime;
        if(followPlayer == true)
        {
            bool isMoving = false;

            if(Vector3.Distance(transform.position, pm.transform.position) > distanceToPlayer && cb.targetIsSixpackfish == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, pm.transform.position, crabSpeed);
                isMoving = true;
            }
            else if(Vector3.Distance(transform.position, sixpackFish.transform.position) > distanceToPlayer && cb.targetIsSixpackfish == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, sixpackFish.transform.position, crabSpeed);
                isMoving = true;
            }

            anim.SetBool("IsMoving", isMoving);
        }
    }
}
