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
	void FixedUpdate () {
        yaw += sensitivity * Input.GetAxis("Mouse X");
        pitch = Mathf.Clamp(pitch + (sensitivity * -Input.GetAxis("Mouse Y")), -90f, 90f);

        camera.transform.rotation = Quaternion.Euler(new Vector3(pitch, yaw, 0f));
        transform.rotation = Quaternion.Euler(new Vector3(0f, yaw, 0f));

		rigidbody.AddRelativeForce(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * movementSpeed);
	}


    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            RaycastHit rh;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out rh, 3))
            {
                if (rh.collider.name == "MainBody")
                {
                    Material dirtMat = rh.collider.gameObject.GetComponent<MeshRenderer>().materials[1];
                    float currentDirt = Mathf.Clamp01(dirtMat.GetFloat("_BlendAmount") - 0.025f);
                    dirtMat.SetFloat("_BlendAmount", currentDirt);
                }
                
            }

        }
    }

    /*void FixedUpdate()
    {
        
    }*/
}
