using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform cameraTarget;
    public float moveSpeed;

    private Vector3 camVelocity;

    public void Start()
    {
        camVelocity = Vector3.zero;
    }

    public void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, cameraTarget.position, ref camVelocity, moveSpeed);
    }
}
