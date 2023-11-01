using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentChanger : MonoBehaviour
{
    public List<GameObject> objectsToShow; // Object referece to the object/obejects we want to show
    public int maxStages = 3; //Number of stages

    private int currentStage = 0; //Defining the current stage. starts at 0

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentStage < maxStages)
            {
                ActivateObjectsForStage(currentStage);
                currentStage++; //functions as a counter for the current stage
            }
        }
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
            }
        }
    }
}
