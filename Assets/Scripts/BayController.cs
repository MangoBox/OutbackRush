﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BayController : MonoBehaviour {

	//Singleton reference
	public static BayController bc;


	//Consider settings these variables as constants later. (GameConstants file)
	public float bayProgressRate = 0.1f;

	//Reference to all station bays. Remember to listen to callbacks.
	public Bay[] bays;


	//TODO: Consider moving below bay UI fields to seperate class in order to organise code.
	public Image[] bayImages;

	public Color startColor;
	public Color finishColor;

	void Awake() {
		bc = this;
        InitBays();
    }

	// Use this for initialization
	void Start () {
       
	}

	void InitBays() {
		foreach (Bay b in bays) {
			b.currentProgress = 1f;
            b.bayFinishEvent += BayFinish;
		}
        
	}
	
	// Update is called once per frame
	void Update () {
        int i = 0;
		foreach(Bay b in bays)
        {
            UpdateBaySprite(bayImages[i++], b.currentProgress, true);
        }
	}

	void UpdateBaySprite(Image baySprite, float prog, bool enabled) {
		if (enabled) {
			baySprite.fillAmount = prog;
			baySprite.color = Color.Lerp (finishColor, startColor, prog);
		} else {
			baySprite.color = new Color (0, 0, 0, 0);
		}
	}

	//A callback for bay classes, notified by Bay class.
	void BayFinish(Bay bay) {
		bay.currentProgress = 1;

		//Notify gameController.
	}

}