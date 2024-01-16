using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixpackFish : MonoBehaviour
{
    private Camera mainCam;                          //Camera
    private PlayerMovementFisk pm;                   //spillerens script
    private Animator anim;                           //animatoren på npc
    [HideInInspector] public bool detectPlayer;       //Bool der holder styr om spilleren er tæt på npcen
    private bool canChangeEnvironment = true;
    private float DialougeDelay = 0.1f;

    [SerializeField] private float waitToMoveTime = 5;
    public GameObject containerTrash;
    public Animator runAnim;

    [Header("State")]
    public state currentState = state.firstmeeting;
    public enum state { earlymeeting, firstmeeting, help, saved }

    // Defineing gameobject variables to put canvas textbox into and
    // makeing them visible in inspector under the "dialogue boxes" header.
    [Header("dialouge boxes")]
    [SerializeField] private Vector3 textOffset;     //offset offset where the text should be placed based of on the NPC
    [SerializeField] private GameObject earlyDialouge;
    [SerializeField] private GameObject helpMeDialouge;
    [SerializeField] private GameObject thankyouDialouge;
    [SerializeField] private GameObject pressToTalk;      //objekt that include the "press M" text. 

    public bool canTalk = true;

    private void Start()
    {
        mainCam = Camera.main;                      //Collects the camera
        anim = GetComponent<Animator>();
        pm = FindAnyObjectByType<PlayerMovementFisk>();

       
        // sets the right pararmeteres to false when the game starts. 
        detectPlayer = false;
        helpMeDialouge.SetActive(false);
        thankyouDialouge.SetActive(false);
        earlyDialouge.SetActive(false);
        pressToTalk.SetActive(false);
   
    }
    //
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
        // Tells the textboxes that they need to be placed at the NPC's position + the given offset
       
        PlaceText();
        CheckState();
    }

    private void CheckState()
    {
        switch (GameManager.instance.currentStage)
        {
            case GameManager.gameStage.stage1:
                currentState = state.earlymeeting;
                break;

            case GameManager.gameStage.stage2:
                currentState = state.earlymeeting;
                break;

            case GameManager.gameStage.stage3:
                currentState = state.firstmeeting;
                break;
        }

        if (currentState == state.saved)
            canTalk = true;

    }

    private void OnTriggerEnter2D(Collider2D other)

    //If the player gets into the collider, the bool will be set to true and the "press m" text will be activated.
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = true;
            pressToTalk.SetActive(true);   
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    //If the player leaves the collider again the text boxes will be turned off.
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = false;
            helpMeDialouge.SetActive(false);
            thankyouDialouge.SetActive(false);
            earlyDialouge.SetActive(false);
            pressToTalk.SetActive(false);
            
        }
    }

    private void PlaceText()
    {
        helpMeDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        earlyDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        thankyouDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);

        pressToTalk.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
    }

    public void Talk()
    {
        // if statement 
        if (detectPlayer && canTalk == true)
        {
            pm.canMove = false;
            pm.canTalk = false;
            pressToTalk.SetActive(false);

            DeactivateChilden(helpMeDialouge);
            DeactivateChilden(earlyDialouge);
            DeactivateChilden(thankyouDialouge); 
            
            switch (currentState)
            {
                case state.earlymeeting:
                    StartCoroutine(ActivateDialogueWithDelay(earlyDialouge));
                    break;

                case state.firstmeeting:
                    StartCoroutine(ActivateDialogueWithDelay(helpMeDialouge));
                    if (GameManager.instance.currentStage == GameManager.gameStage.stage3)
                    {
                        // // tells gamemanager to increase the coralstage and change gamestate to stage 4
                        GameManager.instance.IncreaseCoralStage();
                        GameManager.instance.currentStage = GameManager.gameStage.stage4;
                        containerTrash.SetActive(true);
                    }
                    break;

                case state.saved:
                    StartCoroutine(ActivateDialogueWithDelay(thankyouDialouge));
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

             if(currentState == state.saved && canChangeEnvironment == true)
             {
                canChangeEnvironment = false;
                GetComponent<BoxCollider2D>().enabled = false;
                runAnim.SetTrigger("Swim");
                GameManager.instance.IncreaseCoralStage();
                GameManager.instance.currentStage = GameManager.gameStage.stage6;
             }

            pm.canMove = true;
            pm.canTalk = true;

            Dialogue.SetActive(false);
        }
            
    }
    //Eveything below was a dublicate of the above If statement and is commentet out.

    //private IEnumerator WaitToMove(float waitTime)
    //{
       // yield return new WaitForSeconds(waitTime);

      //  if(currentState == state.saved && canChangeEnvironment == true)
      //  {
     //       canChangeEnvironment = false;
       //     GetComponent<BoxCollider2D>().enabled = false;
       //     runAnim.SetTrigger("Swim");
        //    GameManager.instance.IncreaseCoralStage();
        //    GameManager.instance.currentStage = GameManager.gameStage.stage6;
        //}
    //}
}