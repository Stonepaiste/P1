using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public int PlayerSpeed;

    void Update()
    {
        transform.Translate(Vector2.down * Input.GetAxis("Vertical") * PlayerSpeed * Time.deltaTime);//1.0.0
        transform.Translate(Vector2.right * Input.GetAxis("Horizontal") * PlayerSpeed * Time.deltaTime);//0.0.1
    }
}
