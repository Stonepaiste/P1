using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementFisk : MonoBehaviour
{
    private Vector2 movement; // Kalder Vector2 info og gemmer dem i memory som "movement"
    private Rigidbody2D Fiskekrop; // kalder ridigbofy classen og gemmer den som "fiskekrop"
    public int speed = 5; // SerializeField gør at access modifieren er sat til private men at vi stadig kan se values i unity

    //Referer til input systemet
    [HideInInspector]public InputAction talkAction; //specifikke talk actions
    [SerializeField] private InputActionAsset inputActions; //Input action map
    [SerializeField] private TurtleNPC turtle; //reference til dialogscript
    [SerializeField] private SixpackFish sixpackFish; //reference til dialogscript
    [SerializeField] private CodNPC cod; //reference til dialogscript
    [SerializeField] private KarstenNPC krabbe; //reference til dialogscript
    [SerializeField] private float waitaftertalk = 8;

    public AudioSource swimSound;

    public bool canMove; //can er en variable om spilleren kan bevæge sig når man snakker med NPC'er

    public bool canTalk; //Boolean for om spilleren kan snakke

    public bool turtletalkaudio;  //forskellige boolean for at tænde lyd
    public bool Sixpacktalkaudio;
    public bool codtalkaudio;
    public bool krabbetalkaudio;
    public bool turtlesilence;

    private Animator animator;

    private void Awake() // kører kun 1 gang når program starter inden alt andet
    {
        canMove = true;  //når spillet starter må spilleren bevæge sig
        canTalk = true;    //boolean til at spillere godt kan snakke med NPC

        turtletalkaudio = false;  //skifter alle lyde til false altså fra
        Sixpacktalkaudio = false;
        codtalkaudio = false;
        krabbetalkaudio = true;
        turtlesilence = false;


        animator = GetComponent<Animator>();  //vi får vore Rigidbody 
        Fiskekrop = GetComponent<Rigidbody2D>(); // Vi sætter "fiskekrop" ridigbody til rigidbody på vores gameobject
        talkAction = inputActions.FindActionMap("Player").FindAction("Talk"); //Finder talk inputtet i input mappet i unity
    }

    private void OnMove(InputValue value) // vi laver en funktion der hodler øje med vores input system values
    {
        movement = value.Get<Vector2>(); // vi tag vores vector2 value fra vores input manager

        if (movement.x != 0 || movement.y != 0) // vi tjekker om spilleren bevæger sig
        {
            if (canMove) // hvis vi canmove er true kører næste section
            {
                animator.SetFloat("Horizontal", movement.x); //vi tag x float værdi og sætter ind i vores animator
                animator.SetFloat("Vertical", movement.y);  //vi tag y float værdi og sætter ind i vores animator
                animator.SetBool("IsWalking", true); //vi tag vores boolean for om vi skal labvve run animation til true
                swimSound.Play(); //vi player vores gå lyd
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);   //ellers sætter vi vores boolean til false / idle
            swimSound.Stop(); //lyden bliver stopper
        }

    }

    private void FixedUpdate() //fixed update er bedre til movement da den sætter en fast movement på trods af FPS og CPU power
    {

        if(canMove)
            Fiskekrop.AddForce(movement * speed);  //vi tag vores rigidbody og bruger addforce til at bevæge vores spiller med movement
    }
    
    private void Update()

    {


        //Hvis vores input til at snakke bliver triggered, kalder den snakke action i npc scriptet
        if (talkAction.triggered && canTalk)
        {
            if(turtle != null && turtle.detectPlayer==true)
            { 
                turtle.Talk();
                //turtletalkaudio = true;
                turtle.GetComponent<AudioSource>().Play();
                turtlesilence = true;
                StartCoroutine (Turtlespeakwait());


            }

            if(cod != null && cod.detectPlayer==true)
            { 
                cod.Talk();
                //codtalkaudio = true;
                cod.GetComponent<AudioSource>().Play();
            }

            if(sixpackFish != null && sixpackFish.detectPlayer==true)
            { 
                sixpackFish.Talk();
                //Sixpacktalkaudio = true;
                sixpackFish.GetComponent<AudioSource>().Play();
            }

            if (krabbe != null && krabbe.detectPlayer==true)
            {
                krabbe.Talk();
                // krabbetalkaudio = true;
                krabbe.GetComponent<AudioSource>().Play();
            }


        }
    }
   

  IEnumerator Turtlespeakwait()
       {
            yield return new WaitForSeconds (waitaftertalk);

            if (turtlesilence==true)
            {
                turtle.GetComponent<AudioSource>().enabled = false;

            }
            

       }

      

    
}

