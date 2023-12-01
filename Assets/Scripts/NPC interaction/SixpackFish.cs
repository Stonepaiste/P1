using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixpackFish : MonoBehaviour
{
    private Camera mainCam;                          //Camera
    private PlayerMovementFisk pm;                   //spillerens script
    private Animator anim;                           //animatoren på npc
    [HideInInspector] public bool detectPlayer;       //Bool der holder styr om spilleren er tæt på npcen
   

    [SerializeField] private float waitToMoveTime = 5;
    public GameObject containerTrash;
    public Animator runAnim;

    [Header("State")]
    public state currentState = state.firstmeeting;
    public enum state { earlymeeting, firstmeeting, help, saved }

    [Header("dialouge boxes")]
    [SerializeField] private Vector3 textOffset;     //offset hvor tekst skal placere sig ift. npc'en selv
    [SerializeField] private GameObject earlyDialouge;
    [SerializeField] private GameObject firstDialouge;
    [SerializeField] private GameObject secondDialouge;
    [SerializeField] private GameObject thirdDialouge;
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
        earlyDialouge.SetActive(false);
        pressToTalk.SetActive(false);
        
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
            secondDialouge.SetActive(false);
            thirdDialouge.SetActive(false);
            earlyDialouge.SetActive(false);
            pressToTalk.SetActive(false);
            
        }
    }

    private void PlaceText()
    {
        firstDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        secondDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        earlyDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        thirdDialouge.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);

        pressToTalk.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
    }

    public void Talk()
    {
        if (detectPlayer)
        {
            pressToTalk.SetActive(false);

            StartCoroutine(WaitToMove(waitToMoveTime));
            pm.canMove = false;
            pm.canTalk = false;
            
            switch (currentState)
            {
                case state.earlymeeting:
                    earlyDialouge.SetActive(true);
                    earlyDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    break;

                case state.firstmeeting:
                    firstDialouge.SetActive(true);
                    firstDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    GameManager.instance.DeactivateObjectsForStage(GameManager.instance.currentCoralStage);
                    GameManager.instance.currentCoralStage++;
                    GameManager.instance.ActivateObjectsForStage(GameManager.instance.currentCoralStage);
                    GameManager.instance.currentStage = GameManager.gameStage.stage4;
                    currentState = state.help;
                    containerTrash.SetActive(true);
                    break;

                case state.help:
                    firstDialouge.SetActive(false);
                    secondDialouge.SetActive(true);
                    secondDialouge.GetComponent<Animator>().SetTrigger("Animate");
                    break;

                case state.saved:
                    thirdDialouge.SetActive(true);
                    thirdDialouge.GetComponent<Animator>().SetTrigger("Animate");

                    break;

            }
        }
    }

    private IEnumerator WaitToMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if(currentState == state.saved)
        {
            runAnim.SetTrigger("Swim");
        }

        pm.canMove = true;
        pm.canTalk = true;
    }
}