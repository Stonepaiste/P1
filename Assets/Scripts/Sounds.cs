using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sounds : MonoBehaviour
{
    SixpackFish sixpacktalkactive;
    PlayerMovementFisk pm;

    public AudioSource sixpackfishAudiosource;
    public AudioSource krabbeaudiosource;
    public AudioSource turtleaudiosource;
    public AudioSource codaudiosource;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
        sixpacktalkactive = GetComponent<SixpackFish>();
        pm = GetComponent<PlayerMovementFisk>();
    }


    // Update is called once per frame

    //public bool canPlay = false;
    //void Update()
    //{


    //    if (pm.Sixpacktalkaudio == true && canPlay == true)
    //    {
    //        canPlay = true;
    //        sixpackfishAudiosource.Play();
    //    }

    //    if (pm.krabbetalkaudio && canPlay == true)
    //    {
    //        canPlay = true;
    //        krabbeaudiosource.Play();
    //    }

    //    if (pm.turtletalkaudio && canPlay == true)
    //    {
    //        turtleaudiosource.Play();
    //        canPlay = true;
    //    }

    //    if (pm.codtalkaudio == true && canPlay == true)
    //    {
    //        canPlay = true;
    //        codaudiosource.Play();
    //        Debug.Log("bjafgj");
    //    }


    //}


    void OnTriggerEnter2D(Collider2D other)


    {
        Debug.Log("Collision detected with: " + other.tag);
        if (other.CompareTag("Sixpackfish") && pm.talkAction.triggered)

        {
            sixpackfishAudiosource.Play();

            Debug.Log("can play sound!");

        }


    }
}