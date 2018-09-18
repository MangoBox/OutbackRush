using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInstance : MonoBehaviour {

    float dirtAmount;
    float fuelAmount;

    public MeshRenderer bodyRenderer;

    public int bayNum;

    public void SetAnimationBay(int bay) {
        bayNum = bay;
        GetComponent<Animator>().SetInteger("BayNum", bay);
    }

    public void CarLeave()
    {
        GetComponent<Animator>().SetTrigger("LeaveBay");
    }

    public void Start()
    {
        bodyRenderer.materials[1].color = new Color(Random.value, Random.value, Random.value);
    }

}
