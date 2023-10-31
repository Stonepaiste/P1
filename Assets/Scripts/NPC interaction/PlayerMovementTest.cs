using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementTest : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementValue;
    [SerializeField]private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value)
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
