using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bay : MonoBehaviour {

	public float currentProgress;

    //Callback delegate for GameController. Bay parameter is self pass in GameController.
    public delegate void BayFinishEvent(Bay bay);
	public BayFinishEvent bayFinishEvent;

	// Use this for initialization
	void Start () {
		currentProgress = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentProgress > 0f) {
			currentProgress -= BayController.bc.bayProgressRate * Time.deltaTime;
		} else {
            //Call callback delegate.
            if(bayFinishEvent != null)
                bayFinishEvent(this);
        }
	}
}
