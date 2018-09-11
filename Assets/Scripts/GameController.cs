using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	//Singleton reference
	public static GameController gc;

    public GameObject[] carPrefabs;


    public GameObject carObj;


	void Awake() {
		gc = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    void SpawnCar()
    {
        GameObject pref = carPrefabs[Random.Range(0, carPrefabs.Length)];
        GameObject car = Instantiate(pref);
    }
}
