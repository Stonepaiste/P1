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
    private float DialougeDelay = 0.1f;


    [SerializeField] private float waitToMoveTime = 5;
    public int trashNeeded = 10;
    public DestroyTrashContainer container;
    public bool followPlayer;
    public float speed = 4;
    public float distanceToPlayer = 2;

    private bool canTalk = true;

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
            //other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            savedFish.SetActive(true);
            sixpackFish.currentState = SixpackFish.state.saved;
            currentState = state.finished;
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
        if (detectPlayer && canTalk == true)
        {
            pressToTalk.SetActive(false);

            StartCoroutine(WaitToMove(waitToMoveTime));
            pm.canMove = false;
            pm.canTalk = false;
            DeactivateChilden(helpWithTrash); 
            DeactivateChilden(early); 
            DeactivateChilden(finishedDialouge); 

            switch (currentState)
            {
                case state.early:
                    StartCoroutine(ActivateDialogueWithDelay(early));
                    break;

                case state.firstmeeting:
                    StartCoroutine(ActivateDialogueWithDelay(helpWithTrash));
                    GameManager.instance.IncreaseCoralStage();
                    GameManager.instance.currentStage = GameManager.gameStage.stage5;
                    currentState = state.follow;
                    break;

                case state.follow:
                    if(container.trashcollected >= trashNeeded)
                    {
                        StartCoroutine(ActivateDialogueWithDelay(thankyouDialouge));
                    }
                    else
                    {
                        StartCoroutine(ActivateDialogueWithDelay(helpWithTrashStill));
                    }
                    break;

                case state.finished:
                    StartCoroutine(ActivateDialogueWithDelay(finishedDialouge));
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
            pm.canMove = true;
            pm.canTalk = true;
            Dialogue.SetActive(false);
            if (container.trashcollected >= trashNeeded)
                followPlayer = true;
        }
            
     }

    private IEnumerator WaitToMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (GameManager.instance.currentStage == GameManager.gameStage.stage4)
            GameManager.instance.currentStage = GameManager.gameStage.stage5;
    }

    private void Follow()
    {
        var crabSpeed = speed * Time.deltaTime;
        if(followPlayer == true)
        {
            bool isMoving = false;
            canTalk = true;

            if(Vector3.Distance(transform.position, pm.transform.position) > distanceToPlayer && cb.targetIsSixpackfish == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, pm.transform.position, crabSpeed);
                isMoving = true;
                canTalk = false;
            }
            else if(Vector3.Distance(transform.position, sixpackFish.transform.position) > distanceToPlayer && cb.targetIsSixpackfish == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, sixpackFish.transform.position, crabSpeed);
                isMoving = true;
                canTalk = false;
            }

            anim.SetBool("IsMoving", isMoving);
        }
    }
}
