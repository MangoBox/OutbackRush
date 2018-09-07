using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaylightController : MonoBehaviour {

    [Header("Scene Objects")]
    public Light directionalLight;


    [Header("Lighting Properties")]
    public float daylightRate;
    public AnimationCurve intensityCurve;
    public float maxIntensity;

    float progress = 0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        directionalLight.transform.Rotate(Vector3.up * daylightRate);
        directionalLight.intensity = intensityCurve.Evaluate(directionalLight.transform.rotation.eulerAngles.x / 180f) * maxIntensity;
        
	}
}
