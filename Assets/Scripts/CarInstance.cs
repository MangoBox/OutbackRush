using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInstance : MonoBehaviour {

    float dirtAmount;
    float fuelAmount;

    public int bayNum {
     set {
         bayNum = value;
         setAnimationBay(value);
     }
    }


    void setAnimationBay(int bay) {
        GetComponent<Animator>().SetInteger("BayNum", bay);
    }

    public void CarLeave()
    {
        GetComponent<Animator>().SetTrigger("LeaveBay");
    }

}
