using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrashContainer : MonoBehaviour
{
    public int trashcollected = 0;          //Int der holder øje med hvor meget trash der er collected

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Trash"))      //Tjekker om det er et stykke skrald der har ramt collideren
        {
            trashcollected++;               //Increaser int'en
            Destroy(other.gameObject);      //Ødelægger skraldet
        }
    }
}
