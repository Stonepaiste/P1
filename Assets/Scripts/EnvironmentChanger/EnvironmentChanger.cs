using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentChanger : MonoBehaviour
{
    public List<GameObject> objectsToShow; // Object referece to the object/obejects we want to show
    public int maxStages = 3; //Number of stages

    public Transform specificSide; // Assign the transform of the specific side of the collider.
    public bool checkRight;
    public bool checkDown;
    private int currentStage = 0; // Initialize to 0 to indicate first stage ís active
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (checkDown)
            {
                float relativePosition = other.transform.position.y - specificSide.position.y;
                if (currentStage < maxStages && relativePosition >= 0)
                {
                    // Deactivate objects from the previous stage.
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
                    // Deactivate objects from the previous stage.
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
                    // Deactivate objects from the previous stage.
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

    private void ActivateObjectsForStage(int stage)
    {
        // Activate objects for the given stage.
        if (stage < objectsToShow.Count)
        {
            objectsToShow[stage].SetActive(true);
        }
    }

    private void DeactivateObjectsForStage(int stage)
    {
        // Deactivate objects for the given stage.
        if (stage < objectsToShow.Count)
        {
            objectsToShow[stage].SetActive(false);
        }
    }

}

