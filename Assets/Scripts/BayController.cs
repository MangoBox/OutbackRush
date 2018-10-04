using System.Collections;
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
        int bayNum = 0;
		foreach (Bay b in bays) {
			b.currentProgress = 0f;
            b.bayFinishEvent += BayFinish;
            b.bayNum = bayNum++;
		}
        
	}

	//A callback for bay classes, notified by Bay class.
	void BayFinish(Bay bay) {
        CarInstance carInst = GameController.gc.GetCarByBay(bay.bayNum);
        if (carInst == null) return;
        carInst.CarLeave();
		//Notify gameController.
	}

    //Gets if all bays are either waiting or full
    public bool isFull()
    {
        foreach (Bay b in bays) {
            if(b.carStatus == Bay.CarStatus.EMPTY) return false;
        }
        return true;
    }

    //Gets if bay is in use
    public bool isInUse(int bay)
    {
        return bays[bay].carStatus != Bay.CarStatus.EMPTY;
    }

    public void NotifyBayEnter(CarInstance carInstance)
    {
        bays[carInstance.bayNum].carStatus = Bay.CarStatus.FULL;
        carInstance.OpenBayIndicator();
    }


    public void EmptyBay(int bay)
    {
        bays[bay].carStatus = Bay.CarStatus.EMPTY;
    }
}
