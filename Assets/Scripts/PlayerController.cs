using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Camera camera;
    public Rigidbody rigidbody;

    public float sensitivity;
    public float movementSpeed;

    public float yaw = 0.0f;
    public float pitch = 0.0f;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        yaw += sensitivity * Input.GetAxis("Mouse X");
        pitch = Mathf.Clamp(pitch + (sensitivity * -Input.GetAxis("Mouse Y")), -90f, 90f);

        camera.transform.rotation = Quaternion.Euler(new Vector3(pitch, yaw, 0f));
        transform.rotation = Quaternion.Euler(new Vector3(0f, yaw, 0f));

        

	}

    void FixedUpdate()
    {
        rigidbody.AddRelativeForce(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * movementSpeed);
    }
}
