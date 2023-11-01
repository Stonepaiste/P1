using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menubuttons : MonoBehaviour
{
    public void OnPlay()
    {
        SceneManager.LoadScene(1);       
    }

    public void OnQuit()
    {
        Application.Quit();
        Debug.Log("quit!");
    }

}
