using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    private Vector3 followVelocity = Vector3.zero;
    public float followTime;
    public float minYPos = -35;

    private void LateUpdate()
    {
        Vector3 nextTarget = target.position + offset;

        Vector3 nextposition = Vector3.SmoothDamp(transform.position, nextTarget, ref followVelocity, followTime);

        if (nextposition.y < minYPos)
        {
            nextposition.y = minYPos;
        }

        transform.position = nextposition;
    }
}
