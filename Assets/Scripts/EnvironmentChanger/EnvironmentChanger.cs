using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentChanger : MonoBehaviour
{
    public List<GameObject> objectsToShow; // Object referece til det objekt/objekter der skal vises
    public int maxStages = 3; //antal stages

    public int currentStage = 0; // start på nul for at vise at den første stage er aktiv
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

