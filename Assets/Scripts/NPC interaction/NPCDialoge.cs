using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialoge : MonoBehaviour
{

    private Camera mainCam;                         
    [SerializeField]private Animator anim;          //Animator til tekst
    [SerializeField]private GameObject pressM;      //objekt der indeholder press m tekst
    [SerializeField]private GameObject dialogeBox;  //objekt der indeholder dialogtekst
    [SerializeField]private Vector3 textOffset;     //offset hvor tekst skal placere sig ift. npc'en selv
    [HideInInspector]public bool detectPlayer;      //Bool der holder styr om spilleren er tæt på npcen


    private void Start()
    {
        mainCam = Camera.main;                      //henter kamera

        //Siger til tekstbokse at de skal placere sig ved npc'ens position + det givne offset
        dialogeBox.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);
        pressM.transform.position = mainCam.WorldToScreenPoint(transform.position + textOffset);

        //Sætter de rigtige parametre til false når spillet starter
        detectPlayer = false;
        dialogeBox.SetActive(false);
        pressM.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D other)
    //Hvis spilleren kommer ind i collideren bliver bool sat til true og press m tekst aktiveret
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = true;
            pressM.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    //Hvis spilleren går ud a collideren slukkes der for alt tekst igen
    {
        if (other.CompareTag("Player"))
        {
            detectPlayer = false;
            dialogeBox.SetActive(false);
            pressM.SetActive(false);
        }
    }

    public void Talk()
    //Denne funktion bliver kaldt fra scriptet på spilleren når man trykker M
    //Dialog bliver kun tændt for hvis detectplayer er true.
    {
        if (detectPlayer)
        {
            pressM.SetActive(false);
            anim.SetTrigger("Animate");
            dialogeBox.SetActive(true);
        }
    }
}
