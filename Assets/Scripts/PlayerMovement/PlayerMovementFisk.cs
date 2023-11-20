using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementFisk : MonoBehaviour
{
    private Vector2 movement; // Kalder Vector2 info og gemmer dem i memory som "movement"
    private Rigidbody2D Fiskekrop; // kalder ridigbofy classen og gemmer den som "fiskekrop"
    [SerializeField] private int speed = 5; // SerializeField gør at access modifieren er sat til private men at vi stadig kan se values i unity

    //Referer til input systemet
    private InputAction talkAction; //specifikke talk actions
    [SerializeField] private InputActionAsset inputActions; //Input action map
    [SerializeField] private TurtleNPC turtle; //reference til dialogscript
    [SerializeField] private SixpackFish sixpackFish; //reference til dialogscript
    [SerializeField] private CodNPC cod; //reference til dialogscript
    //[SerializeField] private NPCDialoge sadfish; //reference til dialogscript

    public bool canMove;
    public bool canTalk;

    private Animator animator;

    private void Awake() // kører kun 1 gang når program starter 
    {
        canMove = true;
        canTalk = true;
        animator = GetComponent<Animator>();
        Fiskekrop = GetComponent<Rigidbody2D>(); // Vi sætter "fiskekrop" ridigbody til rigidbody på vores gameobject
        talkAction = inputActions.FindActionMap("Player").FindAction("Talk"); //Finder talk inputtet i input mappet i unity
    }

    private void OnMove(InputValue value) // vi laver en funktion der hodler øje med vores input system values
    {
        movement = value.Get<Vector2>(); // Movement bliver sat til vector2 fra vores input action når bruger trykker WASD

        if (movement.x != 0 || movement.y != 0)
        {
            if (canMove)
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetBool("IsWalking", true);
            }
        }
        else
            animator.SetBool("IsWalking", false);

    }

    private void FixedUpdate() //fixed update er bedre til movement da den sætter en fast movement på trods af FPS og CPU power
    {
        if(canMove)
            Fiskekrop.AddForce(movement * speed);
    }

    private void Update()
    {
        //Hvis vores input til at snakke bliver triggered, kalder den snakke action i npc scriptet
        if (talkAction.triggered && canTalk)
        {
            if(turtle != null)
                turtle.Talk();

            if(cod != null)
                cod.Talk();
            
            if(sixpackFish != null)
                sixpackFish.Talk();

            //if(sadfish != null)
           //     sadfish.Talk();
            
        }
    }
}
