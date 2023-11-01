using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentChanger : MonoBehaviour
{
    public List<GameObject> objectsToShow; // Reference to the objects you want to show.
    public int maxStages = 3; // Set the maximum number of stages here.
    public Transform specificSide; // Assign the transform of the specific side of the collider.

    private int currentStage = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Calculate the player's position relative to the specific side of the collider.
            float relativePosition = other.transform.position.x - specificSide.position.x;

            if (currentStage < maxStages && relativePosition >= 0)
            {
                ActivateObjectsForStage(currentStage);
                currentStage++;
            }
        }
    }

    private void ActivateObjectsForStage(int stage)
    {
        // Activate objects for the given stage.
        for (int i = 0; i <= stage; i++)
        {
            if (i < objectsToShow.Count)
            {
                objectsToShow[i].SetActive(true);
            }
        }
    }
}
