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

    //Checks if a car be spawned, and if so, spawns it and sets bay information.
    void CheckCar()
    {
        if (bayController.isFull()) return;

        int bayNum;
        do
        {
            bayNum = Random.Range(0, 6);
        } while (!bayController.isInUse(bayNum));

        bayController.bays[bayNum].carStatus = Bay.CarStatus.WAITING;

    }

    void SpawnCar(int bay)
    {
        GameObject pref = carPrefabs[Random.Range(0, carPrefabs.Length)];
        GameObject car = Instantiate(pref);
        CarInstance carInst = car.GetComponent<CarInstance>();
        carInst.bayNum = bay;
        allWorldCars.Add(carInst);
    }

    public void NotifyCarReady(CarInstance carInst)
    {
        carInst.CarLeave();
    }
}
