using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform cameraTarget;
    public float moveSpeed;


    public void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTarget.position, moveSpeed);
    }
}
