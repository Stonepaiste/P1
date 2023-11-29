using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarstenCollider : MonoBehaviour
{
    public GameObject savedFish;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sixpackfish"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            savedFish.SetActive(true);
        }
    }

}
