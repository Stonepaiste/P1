using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        }
    }
}
