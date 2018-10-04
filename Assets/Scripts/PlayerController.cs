using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class PlayerController : MonoBehaviour {

    public Camera camera;
    public Rigidbody rigidbody;
    public Animator playerAnimator;

    public float sensitivity;
    public float movementSpeed;

    public float yaw = 0.0f;
    public float pitch = 0.0f;

    public float cleaningSpeed = 0.025f;
	
	// Update is called once per frame
	void FixedUpdate () {
        rigidbody.AddForce(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * movementSpeed); 
	}

    void LateUpdate()
    {
        Vector2 lookDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        if (lookDirection.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.Euler(0, 360 - (Mathf.Rad2Deg * Mathf.Atan2(lookDirection.y, lookDirection.x) - 90), 0);
        }

        float animParam = (lookDirection.magnitude + 1f) / 2f;
        playerAnimator.SetFloat("Vertical", animParam);

    }


    void Update() {


        if (Input.GetKey(KeyCode.Mouse0))
        {
            RaycastHit rh;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out rh))
            {
                print(rh.collider.name);
                if (rh.collider.name == "MainBody")
                {
                    /*if (Vector3.Distance(rh.collider.ClosestPoint(transform.position), transform.position) > 5)
                        return;*/
                    Material dirtMat = rh.collider.gameObject.GetComponent<MeshRenderer>().materials[1];
                    float currentDirt = Mathf.Clamp01(dirtMat.GetFloat("_BlendAmount") - cleaningSpeed * Time.deltaTime);
                    if (currentDirt <= 0)
                    {
                        //Car is completely clean
                        BayController.bc.NotifyCarReady(rh.collider.GetComponentInParent<CarInstance>());
                    }
                    dirtMat.SetFloat("_BlendAmount", currentDirt);
                }
                
            }

        }
    }
}
