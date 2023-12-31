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

    [Header("dialouge boxes")]
    [SerializeField] private Vector3 textOffset;     //offset hvor tekst skal placere sig ift. npc'en selv
    [SerializeField] private GameObject earlyDialouge;
    [SerializeField] private GameObject helpMeDialouge;
    [SerializeField] private GameObject thankyouDialouge;
    [SerializeField] private GameObject pressToTalk;      //objekt der indeholder press m tekst

    public bool canTalk = true;

    private void Start()
    {
        mainCam = Camera.main;                      //henter kamera
        anim = GetComponent<Animator>();
        pm = FindAnyObjectByType<PlayerMovementFisk>();

        //Sætter de rigtige parametre til false når spillet starter
        detectPlayer = false;
        helpMeDialouge.SetActive(false);
        thankyouDialouge.SetActive(false);
        earlyDialouge.SetActive(false);
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

    private IEnumerator WaitToMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if(currentState == state.saved && canChangeEnvironment == true)
        {
            canChangeEnvironment = false;
            GetComponent<BoxCollider2D>().enabled = false;
            runAnim.SetTrigger("Swim");
            GameManager.instance.IncreaseCoralStage();
            GameManager.instance.currentStage = GameManager.gameStage.stage6;
        }
    }
}