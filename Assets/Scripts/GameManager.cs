using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NPCDialoge sixPackFish;
    public NPCDialoge turtle;
    public NPCDialoge cod;
    public NPCDialoge sadFish;

    public PlayerMovementFisk pm;

    public enum gameStage { stage1, stage2, stage3, stage4, stage5, stage6 }

    public gameStage currentStage = gameStage.stage1;


    void Start()
    {
        pm = FindAnyObjectByType<PlayerMovementFisk>();
    }

    void Update()
    {
        if (cod.currentState == NPCDialoge.state.dead)
        {
            currentStage = gameStage.stage2;
            Debug.Log(currentStage);
        }

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
        }
    }
}
