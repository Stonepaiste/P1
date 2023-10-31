using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementTest : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputAction talkAction;
    private Vector2 movementValue;

    [SerializeField]private InputActionAsset inputActions;
    [SerializeField]private NPCDialoge NPCDialoge;
    [SerializeField]private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        talkAction = inputActions.FindActionMap("Player").FindAction("Talk"); 
    }

    private void Update()
    {
        if (talkAction.triggered)
        {
            NPCDialoge.Talk();
        }
    } 

   public void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movementValue.Normalize();
        rb.velocity = movementValue * moveSpeed;
    }
}
