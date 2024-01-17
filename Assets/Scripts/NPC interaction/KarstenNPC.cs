using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class KarstenNPC : MonoBehaviour
{
    private Camera mainCam;                          //Camera
    private PlayerMovementFisk pm;                   //spillerens script
    private CollideBehavior cb;                      // noget med sixpackfish
    private SixpackFish sixpackFish;                 //sixpackfish
    [HideInInspector] public bool detectPlayer;       //Bool der holder styr om spilleren er tæt på npcen
    private Animator anim;
    public GameObject savedFish;                      // Gameobject med sprite uden sixpackplastik
    private float DialougeDelay = 0.1f;               //


    [SerializeField] private float waitToMoveTime = 5;
    public int trashNeeded = 10;                        //Hvor meget skrald spilleren skal samle for at progresse
    public DestroyTrashContainer container;             //Script på container
    public bool followPlayer;                           //Bool der styrer om krabbe skal følge efter
    public float speed = 4;                             //Movespeed når krabbe følger
    public float distanceToPlayer = 2;                  //Distancen krabbe holder når den følger efter

    private bool canTalk = true;                        // styrer om krabben kan snakke

    // Interne state (NPC'en egen state)
    [Header("State")]
    public state currentState = state.firstmeeting;

    // de 4 states Karsten NPC har.
    // Early hvis man kommer inden gamestate4
    //Firstmeeting ved gamestate 4
    // Follow når man har hjulpet ham med skraldet
    // Finished efter han har fulgt en og man har hjulpet sixpackfisken. 
    public enum state { early, firstmeeting, follow, finished }

    [Header("dialouge boxes")] //gameobjects med dialoger
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

    // Defineing a method to turn off all the text boxes
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
    // I hvert update tjekker checkstate Gamemanagers current stage og da Karsten her har firstmeeting på gamemanagerns stage 4
    // sætter den NPC state til firstmeeting
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
                // hvis gamemanagers current state er "state4" så bliver karsten NPC state sat til firstmeeting
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

        //nedenunder tjekker vi om krappen møder sixpackfisken og om der stadig snakkes for så at skifte state til finished. 
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
    // definering af positionen af teksten bliver først kaldt nede i IEnumerator linie 210
    private void PlaceText()
    {
        helpWithTrash.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        early.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        finishedDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        pressToTalk.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        thankyouDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        helpWithTrashStill.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
    }

    // if statement that starts a switch statement if our player has collied & cantalk = true
    // And "M" is triggered inside the playermovement script. 


    public void Talk()
    {
        if (detectPlayer && canTalk == true)
        {
            pressToTalk.SetActive(false);

           // StartCoroutine(WaitToMove(waitToMoveTime));
            pm.canMove = false;
            pm.canTalk = false; // turns off the pm.cantalk which is part of the if statement that activates this Talk() method.
                                // this happens so we can't start the switch statement all over again before the next NPC state has been actiavted.
            DeactivateChilden(helpWithTrash); 
            DeactivateChilden(early); 
            DeactivateChilden(finishedDialouge); 

            // Switch statement der starter de rigtige textbokse ud fra NPC'ens state. 
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
                         
                        sixpackFish.canTalk = false;
                    }
                    else
                    {
                        StartCoroutine(ActivateDialogueWithDelay(helpWithTrashStill));
                    }
                    break;

                case state.finished:
                    sixpackFish.canTalk = true;
                    StartCoroutine(ActivateDialogueWithDelay(finishedDialouge));
                    break;
            }

            
        }
    }

    // Our started couroutine with our activated gameobject that now goes through textbox transform placement and makes
    // that the remaining dialogue from that child shows when hitting "M"  
     private IEnumerator ActivateDialogueWithDelay(GameObject Dialogue)
     {
        // tjekker om vores Dialogue gameobjekt er intialiseret med noget. Hvis det ikke er tomt != kører den If statment. 
        if (Dialogue != null)
        {
            // Aktiverer gameobjektet der er injekseret i vores Dialogue variabel.
            //Det referer til de definerede gamebojects før start()
            Dialogue.SetActive(true);

            //fortæller hvor teksten skal placeres via placetextmetoden på linje 147.
            foreach (Transform child in Dialogue.transform)
            {
                //hvis både parent gameobjet og child er initialiseret i vores Dialogue variabel kører den videre.
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

   // private IEnumerator WaitToMove(float waitTime)
    //{
      //  yield return new WaitForSeconds(waitTime);

        //if (GameManager.instance.currentStage == GameManager.gameStage.stage4)
          //  GameManager.instance.currentStage = GameManager.gameStage.stage5;


//    }


    // Krabbens move script
    //tjekker om vores vores followplaer bool er true 
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
