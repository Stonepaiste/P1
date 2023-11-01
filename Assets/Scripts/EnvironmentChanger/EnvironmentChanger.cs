using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentChanger : MonoBehaviour
{
    public List<GameObject> objectsToShow; // Object referece to the object/obejects we want to show
    public int maxStages = 3; //Number of stages

    public Transform specificSide; // Assign the transform of the specific side of the collider.
    private int currentStage = -1; // Initialize to -1 to indicate no stage is active.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Calculate the player's position relative to the specific side of the collider.
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

