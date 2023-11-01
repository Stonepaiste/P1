using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentChanger : MonoBehaviour
{
    public List<GameObject> objectsToShow; // Object referece to the object/obejects we want to show
    public int maxStages = 3; //Number of stages

    public Transform specificSide; // Assign the transform of the specific side of the collider.

    private int currentStage = 0; //Defining the current stage. starts at 0
    private List<GameObject> objectsActivatedInPreviousStages = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Calculate the player's position relative to the specific side of the collider.
            float relativePosition = other.transform.position.x - specificSide.position.x;

            if (currentStage < maxStages && relativePosition >= 0)
            {
                DeactivateObjectsFromPreviousStages();
                ActivateObjectsForStage(currentStage);
                currentStage++;

            }

        }

    }

    private void DeactivateObjectsFromPreviousStages()
    {
        // Deactivate objects from previous stages.
        foreach (GameObject obj in objectsActivatedInPreviousStages)
        {
            obj.SetActive(false);
        }

        objectsActivatedInPreviousStages.Clear();
    }
    private void ActivateObjectsForStage(int stage)
    {
        // i is a variable that starts at 0 and loops through
        // Activate objects for the given stage. by looking through the object to show list.
        for (int i = 0; i <= stage; i++) //loops through stages
        {
            if (i < objectsToShow.Count) //checks if i is less than the total objects to show list this case 3
            {
                objectsToShow[i].SetActive(true);
                objectsActivatedInPreviousStages.Add(objectsToShow[i]);
            }
        }
    }

    

}

