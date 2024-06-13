using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float movingSpeed = .125f;
    public Vector3 cameraOffset;
    private void LateUpdate()
    {
        Vector3 desiredPostion = target.position + cameraOffset;
        Vector3 smoothedPostion = Vector3.Lerp(transform.position, desiredPostion, movingSpeed);
        transform.position = smoothedPostion;
        transform.LookAt(target);
    }
}
