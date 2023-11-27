using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> objectsToShow; // Object referece til det objekt/objekter der skal vises
    //public int maxStages = 3; //antal stages

    public int currentCoralStage = 0; // start på nul for at vise at den første stage er aktiv
    public int previousCoralStage;

    public enum gameStage { stage1, stage2, stage3, stage4, stage5, stage6, stage7 }

    public gameStage currentStage = gameStage.stage1;

    public static GameManager instance;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        switch (currentStage)
        {
            case gameStage.stage1:
                break;

            case gameStage.stage2:
                break;

            case gameStage.stage3:
                break;

            case gameStage.stage4:
                break;

            case gameStage.stage5:
                break;

            case gameStage.stage6:
                break;

            case gameStage.stage7:
                break;
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (checkDown)
            {
                float relativePosition = other.transform.position.y - specificSide.position.y;
                if (currentStage < maxStages && relativePosition >= 0)
                {
                    // Deactivate objekter fra den sidte stage.
                    if (currentStage >= 0)
                    {
                        DeactivateObjectsForStage(currentStage);
                    }

                    currentStage++;
                    if (currentStage < objectsToShow.Count)
                    {
                        ActivateObjectsForStage(currentStage);
                    }
                }
            }
            if (checkRight) 
            {
                float relativePosition = other.transform.position.x - specificSide.position.x;
                if (currentStage < maxStages && relativePosition >= 0)
                {
                    // Deactivate objekter fra den sidte stage.
                    if (currentStage >= 0)
                    {
                        DeactivateObjectsForStage(currentStage);
                    }

                    currentStage++;
                    if (currentStage < objectsToShow.Count)
                    {
                        ActivateObjectsForStage(currentStage);
                    }
                }
            }

            if (!checkRight)
            {
                float relativePosition = other.transform.position.x - specificSide.position.x;
                if (currentStage < maxStages && relativePosition <=0)
                {
                    // Deactivate objekter fra den sidte stage.
                    if (currentStage >= 0)
                    {
                        DeactivateObjectsForStage(currentStage);
                    }

                    currentStage++;
                    if (currentStage < objectsToShow.Count)
                    {
                        ActivateObjectsForStage(currentStage);
                    }
                }
            }

        }

    }
    */
    public void ActivateObjectsForStage(int stage)
    {
        // Activate objekter fra den stage vi er kommet til.
        if (stage < objectsToShow.Count)
        {
            objectsToShow[stage].SetActive(true);
        }
    }

    public void DeactivateObjectsForStage(int stage)
    {
        // Activate objekter fra den stage vi er kommet til.
        if (stage < objectsToShow.Count)
        {
            objectsToShow[stage].SetActive(false);
        }
    }
}
