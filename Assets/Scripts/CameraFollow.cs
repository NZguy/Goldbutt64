using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public bool cameraSnapFlag;

    private void Start()
    {
        cameraSnapFlag = false;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPostion = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        if (cameraSnapFlag)
        {
            transform.position = desiredPosition;
            cameraSnapFlag = false;
        }
        else
        {
            transform.position = smoothedPostion;
        }
    }
}
