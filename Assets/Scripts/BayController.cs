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
	public Bay[] bays = new Bay[6];

	public Color startColor;
	public Color finishColor;

	void Awake() {
		bc = this;
        InitBays();
    }

	void InitBays() {
        for (int i = 0; i < 5; i++)
        {
            bays[i] = new Bay();
            bays[i].currentProgress = 0f;
            bays[i].bayNum = i;
        }
	}

    void Update()
    {
        foreach (Bay b in bays)
        {
            if(b.carStatus != Bay.CarStatus.FULL) return;
            b.currentProgress = Mathf.Clamp01(b.currentProgress - (bayProgressRate * Time.deltaTime));
            if (b.currentProgress <= 0)
            {
                BayFinish(b);
            }
        }
    }

	//A callback for bay classes, notified by Bay class.
	void BayFinish(Bay bay) {
        CarInstance carInst = GetCarByBay(bay.bayNum);
        if (carInst == null) return;
        carInst.CarLeave();
		//Notify gameController.
        DeallocateBay(carInst.bayNum);
	}


    public CarInstance GetCarByBay(int bay)
    {
        return bays[bay].currentCar;
    }

    public void AllocateBay(int bay, CarInstance carInst)
    {
        Bay b = bays[bay];
        b.currentCar = carInst;
        b.currentProgress = 1f;
        b.carStatus = Bay.CarStatus.WAITING;

        carInst.SetAnimationBay(bay);
        carInst.bayNum = bay;
    }

    public void DeallocateBay(int bay)
    {
        Bay b = bays[bay];
        b.carStatus = Bay.CarStatus.EMPTY;
        b.currentCar = null;
    }


    public void NotifyCarReady(CarInstance carInst)
    {
        carInst.CarLeave();
        bays[carInst.bayNum].carStatus = Bay.CarStatus.EMPTY;
    }

    public void NotifyCarStationEnter(CarInstance carInst)
    {
        bays[carInst.bayNum].currentProgress = 1f;
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

    //NEW METHODS
}
