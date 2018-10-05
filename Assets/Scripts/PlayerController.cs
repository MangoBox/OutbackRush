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

    public float cleaningSpeed = 15f;

    CarInstance lastCarCleaned;
	
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit rh;
            if (Physics.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), camera.transform.forward, out rh))
            {
                if (rh.collider.name == "MainBody")
                {
                    if (Vector3.Distance(rh.collider.ClosestPoint(transform.position), transform.position) > 5)
                        return;
                    Material dirtMat = rh.collider.gameObject.GetComponent<MeshRenderer>().materials[1];
                    float currentDirt = Mathf.Clamp01(dirtMat.GetFloat("_BlendAmount") - (1 / cleaningSpeed));
                    CarInstance carTarget = rh.collider.GetComponentInParent<CarInstance>();
                    lastCarCleaned = carTarget;
                    carTarget.isCleaning = true;
                    carTarget.cleaningParticles.Emit(5);
                    if (currentDirt <= 0)
                    {
                        if (carTarget.thisBay == null) return;
                        //Car is completely clean
                        BayController.bc.NotifyBayCleaned(carTarget.thisBay);
                    }
                    dirtMat.SetFloat("_BlendAmount", currentDirt);
                    BayController.bc.currentCleaningBay = carTarget.thisBay;
                }
                else
                {
                    BayController.bc.currentCleaningBay = null;
                }

            }

        }
        else
        {
            if(lastCarCleaned != null) lastCarCleaned.isCleaning = false;
            BayController.bc.currentCleaningBay = null;
        }
    }
}
