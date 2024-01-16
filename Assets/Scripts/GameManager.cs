using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Post processing code inspired by youtube video: https://www.youtube.com/watch?v=JF4t9pNaZxg

public class GameManager : MonoBehaviour
{
    [Header("Post Processing")]             // Header bliver brugt til at lave en overskrift i unity inspector 
    public PostProcessVolume ppVolume;      //postprocess
    ColorGrading colorGrading;
    public int satStage3;                   //Parametre bliver sat i inspector
    public int satStage4;
    public int satStage5;
    public int satStage6;
    private int saturationValue;

    [Header("Coral parameters")]
    public int currentCoralStage = 0;       // start på nul for at vise at den første stage er aktiv
    //public int previousCoralStage;        //BLIVER IKKE BRUGT    
    public List<GameObject> objectsToShow;  // Object referece til det objekt/objekter der skal vises

    public enum gameStage { stage1, stage2, stage3, stage4, stage5, stage6, stage7 }    // laver alle de stages vi skal bruge
    public gameStage currentStage = gameStage.stage1;                                   // laver en variabel som indeholder den current stage


    [Header("End video parameters")]        
    public float videoTransition = 0.5f;    //Hvor lang tid den bruger
    public int videoWaitTime;                 // Samme længde som videoen
    public GameObject endVideo;                 // gameobject der indeholder video
    public float imageFadeTime;                 // Hvor lang tid det skal tage at fade til video
    public Image fadeImage;                     //gameobject der indeholder sort billede
    public GameObject lastTurtleDialouge;       //skildpaddedialog "..."
    
    // gør sådan at der er kun er 1 instance af gamemanageren af gangen.. Called Singleton pattern 
    public static GameManager instance;


    void Start()
    {
        instance = this;
        ppVolume.profile.TryGetSettings(out colorGrading);
        colorGrading.active = true;
        endVideo.SetActive(false);
    }

    void Update()
    {
        switch (currentStage)
        {
            case gameStage.stage1:
                break;

            case gameStage.stage2:
                saturationValue = satStage3;
                break;

            case gameStage.stage3:
                break;

            case gameStage.stage4:
                saturationValue = satStage4;
                break;

            case gameStage.stage5:
                saturationValue = satStage5;
                break;

            case gameStage.stage6:
                saturationValue = satStage6;
                break;

            case gameStage.stage7:
                StartCoroutine(EndVideo());
                break;
        }
        colorGrading.saturation.value = saturationValue;
    }

    // når vi kalder increatecoralstage() i NPC scriptet kører nedenstående kode.
    // Currentstage slukker det aktive coralstage i vores list currentCoralStage++ går videre til næste element i listen
    //objectsToShow set active tænder for det nye stage.
    public void IncreaseCoralStage()
    {
        objectsToShow[currentCoralStage].SetActive(false);
        currentCoralStage++;
        objectsToShow[currentCoralStage].SetActive(true);
    }

    public void StartVideo()
    {
        StartCoroutine(EndVideo());
    }

    // Hører til startcoroutine metoden og er den interface der skifter to next. læs lige lidt mere https://habr.com/en/articles/684938/
    public IEnumerator EndVideo()
    {
        float targetAlpha = 1.0f;                   //Definerer vores target alpha, i RGBA
        Color curColor = fadeImage.color;           //Laver en variabel for billedets farve

        while (Mathf.Abs(curColor.a - targetAlpha) > 0.0001f)   //Tjekker hvor tæt på, billedets alpha, er på targetalpha value'en
        {
            //Debug.Log(fadeImage.material.color.a);
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, imageFadeTime * Time.deltaTime); // Tager billedets nuværende farve og ændre alphaen mod target alpha over tiden "imagefadetime". Lerp tager en værdi a og rykker den mod b over en given tid t.
            fadeImage.color = curColor;
            yield return null;
        }

        lastTurtleDialouge.SetActive(false);
        if(endVideo != null)
            endVideo.SetActive(true);       // starter videoen
            
        yield return new WaitForSeconds(videoTransition);   //Venter på at slukke for billedet
        fadeImage.gameObject.SetActive(false);              //Slukker billedet
            

        yield return new WaitForSeconds(videoWaitTime);     //Venter videoens længde på at loade main menu
        SceneManager.LoadScene(0);                          //Loader main menu
    }

}