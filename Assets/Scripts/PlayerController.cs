﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class PlayerController : MonoBehaviour {

    public Camera camera;
    public Rigidbody rigidbody;

    public float sensitivity;
    public float movementSpeed;

    public float yaw = 0.0f;
    public float pitch = 0.0f;

    public float cleaningSpeed = 0.025f;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rigidbody.AddRelativeForce(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * movementSpeed); 
	}

    void LateUpdate()
    {
        yaw += sensitivity * Input.GetAxis("Mouse X");
        pitch = Mathf.Clamp(pitch + (sensitivity * -Input.GetAxis("Mouse Y")), -90f, 90f);

        /*camera.transform.rotation = Quaternion.Euler(new Vector3(pitch, yaw, 0f));*/
        transform.rotation = Quaternion.Euler(new Vector3(0f, yaw, 0f));
    }


    void Update() {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            RaycastHit rh;
            if (Physics.Raycast(transform.position, transform.forward, out rh, 3))
            {
                if (rh.collider.name == "MainBody")
                {
                    Material dirtMat = rh.collider.gameObject.GetComponent<MeshRenderer>().materials[1];
                    float currentDirt = Mathf.Clamp01(dirtMat.GetFloat("_BlendAmount") - cleaningSpeed * Time.deltaTime);
                    if (currentDirt <= 0)
                    {
                        //Car is completely clean
                        GameController.gc.NotifyCarReady(rh.collider.GetComponentInParent<CarInstance>());
                    }
                    dirtMat.SetFloat("_BlendAmount", currentDirt);
                }
                
            }

        }
    }
}
