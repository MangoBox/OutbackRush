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

    //Assigned when player is cleaning a car. Set to null when player is not cleaning a car.
    public Bay currentCleaningBay;

    void Awake() {
        bc = this;

        bays = new Bay[6];
        for (int i = 0; i < 6; i++)
        {
            bays[i] = new Bay();
            bays[i].bayNum = i;
        }
    }

    //Allows a bay to be entered, by setting it to empty.
    public void AllocateBay(int bayNum, CarInstance carInstance)
    {
        AllocateBay(bays[bayNum], carInstance);
    }
    public void AllocateBay(Bay bay, CarInstance carInstance)
    {
        bay.carStatus = Bay.CarStatus.WAITING;
        bay.currentCar = carInstance;
        bay.currentProgress = 1f;

        carInstance.SetAnimationBay(bay.bayNum);
        carInstance.thisBay = bay;
    }

    public void DeallocateBay(int bayNum)
    {
        DeallocateBay(bays[bayNum]);
    }
    public void DeallocateBay(Bay bay)
    {
        bay.carStatus = Bay.CarStatus.EMPTY;
        bay.currentCar.CarLeave();
        bay.currentCar.thisBay = null;
        bay.currentCar = null;
    }

    public void NotifyBayEntered(Bay bay, CarInstance carInstance)
    {
        if (bay == null) return;
        bay.carStatus = Bay.CarStatus.FULL;
    }

    public void NotifyBayExpired(Bay bay)
    {
        //notify GameController
        DeallocateBay(bay);
        GameController.gc.CarAngered();
        GameController.gc.soundManager.Play("FailDing");
    }

    public void NotifyBayCleaned(Bay bay)
    {
        //notify GameController
        DeallocateBay(bay);
        GameController.gc.CarServed();
        GameController.gc.NotifyAllEmpty();
        GameController.gc.soundManager.Play("Ding");
    }

    public void Update()
    {
        foreach (Bay b in bays)
        {
            if (b.carStatus != Bay.CarStatus.FULL) continue;
            if (b == currentCleaningBay) continue;
            //implement checking if player is clicking on car here.
            b.currentProgress = Mathf.Clamp01(b.currentProgress - (bayProgressRate * Time.deltaTime));
            b.currentCar.UpdateBayIndicator(b.currentProgress);
            if (b.currentProgress <= 0)
            {
                NotifyBayExpired(b);
            }
        }
    }

    public bool IsBayInUse(Bay bay)
    {
        return bay.carStatus != Bay.CarStatus.EMPTY;
    }

    public bool IsBaysFull()
    {
        foreach (Bay b in bays)
        {
            if (b.carStatus == Bay.CarStatus.EMPTY) return false;
        }
        return true;
    }

    public bool IsBaysEmpty()
    {
        foreach (Bay b in bays)
        {
            if (b.carStatus != Bay.CarStatus.EMPTY) return false;
        }
        return true;
    }

    public void EmptyAllBays()
    {
        foreach (Bay b in bays)
        {
            if (b.currentCar != null)
                b.currentCar.CarDestroy();
            b.carStatus = Bay.CarStatus.EMPTY;
            if (currentCleaningBay != null)
            {
                if (currentCleaningBay.currentCar != null)
                {
                    currentCleaningBay.currentCar.CarDestroy();
                }
            }
        }
    }

    //Finds a bay number to spawn in. Returns -1 if none
    public int FindBayToSpawn()
    {
        if (IsBaysFull()) return -1;
        int bayNum = 0;
        do
        {
            bayNum = Random.Range(0, 6);
        } while (IsBayInUse(bays[bayNum]));
        return bayNum;
    }
}
