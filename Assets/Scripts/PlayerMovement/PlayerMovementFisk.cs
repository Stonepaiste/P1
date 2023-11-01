using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovementFisk : MonoBehaviour

{

    private Vector2 movement; // Kalder Vector2 info og gemmer dem i memory som "movement"

    private Rigidbody2D Fiskekrop; // kalder ridigbofy classen og gemmer den som "fiskekrop"



    [SerializeField] private int speed = 5; // SerializeField gør at access modifieren er sat til private men at vi stadig kan se values i unity




    private void Awake() // kører kun 1 gang når program starter 
    {
        Fiskekrop = GetComponent<Rigidbody2D>(); // Vi sætter "fiskekrop" ridigbody til rigidbody på vores gameobject
    }

    private void OnMovement(InputValue value) // vi laver en funktion der hodler øje med vores input system values

    {
        movement = value.Get<Vector2>(); // Movement bliver sat til vector2 fra vores input action når bruger trykker WASD

    }

   

    private void FixedUpdate() //fixed update er bedre til movement da den sætter en fast movement på trods af FPS og CPU power
    {
        //Fiskekrop.velocity = movement * speed;
        Fiskekrop.AddForce(movement * speed);
    }

     

    

}
