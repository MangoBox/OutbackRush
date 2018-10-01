using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	//Singleton reference
	public static GameController gc;

    public GameObject[] carPrefabs;
    public BayController bayController;

    public List<CarInstance> allWorldCars = new List<CarInstance>();

    public GameObject carObj;

	void Awake() {
		gc = this;
	}
    
    void Start() {
        InvokeRepeating("CheckCar", 0.3f, 2f);
    }

    //Checks if a car be spawned, and if so, spawns it and sets bay information.
    void CheckCar() {
        if (bayController.isFull()) return;

        int bayNum;
        do
        {
            bayNum = Random.Range(0, 6);
        } while (bayController.isInUse(bayNum));

        bayController.bays[bayNum].carStatus = Bay.CarStatus.WAITING;
        SpawnCar(bayNum);
    }

    void SpawnCar(int bay) {
        GameObject pref = carPrefabs[Random.Range(0, carPrefabs.Length)];
        GameObject car = Instantiate(pref);
        CarInstance carInst = car.GetComponent<CarInstance>();
        carInst.SetAnimationBay(bay);
        allWorldCars.Add(carInst);
    }

    public void NotifyCarReady(CarInstance carInst) {
        carInst.CarLeave();
        bayController.bays[carInst.bayNum].carStatus = Bay.CarStatus.EMPTY;
        bayController.bays[carInst.bayNum].currentProgress = 0;
    }

    public void NotifyCarStationEnter(CarInstance carInst)
    {
        //bayController.bays[carInst.bayNum].carStatus = Bay.CarStatus.FULL;
        bayController.bays[carInst.bayNum].currentProgress = 1f;
    }

    public CarInstance GetCarByBay(int bay)
    {
        return allWorldCars.Find(c => c.bayNum == bay);
    }
}
