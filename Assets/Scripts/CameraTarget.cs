using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public Transform target;                                //Er vores spiller
    public Vector3 offset;                                  //vector 3 fordi kameraet skal ligge bagved spilleren. (z = -10)
    private Vector3 followVelocity = Vector3.zero;          
    public float followTime;                                //Hvor hurtigt kameraet skal følge
    public float minYPos = -35;                             //Hvor langt kameraet kan bevæge nedad.

    private void LateUpdate()                               //Bliver kaldt efter Update og FixedUpdate
    {
        Vector3 nextTarget = target.position + offset;      //Den næste target er spillerens pos(target) + offsettet

        //Vi laver en ny vector som vil blive brugt til den næste position

        Vector3 nextposition = Vector3.SmoothDamp(transform.position, nextTarget, ref followVelocity, followTime);      //Smoothdamp ændrer kameraets position til at gå "smoothly" mod nexttarget vectoren

        if (nextposition.y < minYPos)       //Holder øje med om den næste position er mindre end minYPos og ændre den, hvis den er mindre
        {
            nextposition.y = minYPos;       //Den fasteholder y positionen til -35 hvis true
        }

        transform.position = nextposition;      //Her sætter vi faktisk positionen


        //Forbedring kunne være at have en maks værdi på toppen også. Så fx. +35
    }
}