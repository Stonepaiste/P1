using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Post processing code inspired by youtube video: https://www.youtube.com/watch?v=JF4t9pNaZxg

public class GameManager : MonoBehaviour
{
    [Header("Post Processing")]
    public PostProcessVolume ppVolume;
    ColorGrading colorGrading;
    public int satStage3;
    public int satStage4;
    public int satStage5;
    public int satStage6;
    private int saturationValue;

    [Header("Coral parameters")]
    public int currentCoralStage = 0; // start på nul for at vise at den første stage er aktiv
    public int previousCoralStage;
    public List<GameObject> objectsToShow; // Object referece til det objekt/objekter der skal vises
                                           //public int maxStages = 3; //antal stages

    public enum gameStage { stage1, stage2, stage3, stage4, stage5, stage6, stage7 }
    public gameStage currentStage = gameStage.stage1;


    [Header("End video parameters")]
    public float videoTransition = 0.5f;
    public int videoWaitTime = 3;
    public GameObject endVideo;
    public float imageFadeTime;
    public Image fadeImage;
    public GameObject lastTurtleDialouge;
    

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

    public IEnumerator EndVideo()
    {
        float targetAlpha = 1.0f;
        Color curColor = fadeImage.color;

        while (Mathf.Abs(curColor.a - targetAlpha) > 0.0001f)
        {
            //Debug.Log(fadeImage.material.color.a);
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, imageFadeTime * Time.deltaTime);
            fadeImage.color = curColor;
            yield return null;
        }

        lastTurtleDialouge.SetActive(false);
        if(endVideo != null)
            endVideo.SetActive(true);
            
        yield return new WaitForSeconds(videoTransition);
        fadeImage.gameObject.SetActive(false);
        

        yield return new WaitForSeconds(videoWaitTime);
        SceneManager.LoadScene(0);
    }

}
