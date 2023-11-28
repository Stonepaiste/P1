using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGrav : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float stopGravLeast = 2;
    [SerializeField] private float stopGravMost = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(EndGrav());
    }

    private IEnumerator EndGrav()
    {     
        yield return new WaitForSeconds(Random.Range(stopGravLeast, stopGravMost));
        rb.gravityScale = 0;
    }

}
