using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bay {


    public int bayNum;
    public enum CarStatus
    {
        EMPTY,
        WAITING,
        FULL
    }
    public CarStatus carStatus = CarStatus.EMPTY;
	public float currentProgress;

    public CarInstance currentCar;
}
