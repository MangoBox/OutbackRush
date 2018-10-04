using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarInstance : MonoBehaviour {

    float dirtAmount;
    float fuelAmount;

    public MeshRenderer bodyRenderer;
    public Transform bayIndicatorPosition;
    [HideInInspector]
    Image baseBayIndicator;
    [HideInInspector]
    Image filledBayIndicator;
    [HideInInspector]
    Image faceBayIndicator;

    public int bayNum;

    public void SetAnimationBay(int bay) {
        bayNum = bay;
        GetComponent<Animator>().SetInteger("BayNum", bay);
    }

    public void CarLeave()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("LeaveBay");
        }
        CloseBayIndicator();
        GameController.gc.allWorldCars.Remove(this);
        BayController.bc.EmptyBay(bayNum);
    }

    public void FinishBayEnter()
    {
        BayController.bc.NotifyBayEnter(this);
        OpenBayIndicator();
    }

    public void Start()
    {
        bodyRenderer.materials[1].color = new Color(Random.value, Random.value, Random.value);
        GameObject spawnedIndicator = Instantiate(GameController.gc.bayIndicatorPrefab);
        spawnedIndicator.transform.SetParent(this.transform, false);
        spawnedIndicator.transform.position = bayIndicatorPosition.position;
        baseBayIndicator = spawnedIndicator.GetComponentsInChildren<Image>()[0];
        filledBayIndicator = spawnedIndicator.GetComponentsInChildren<Image>()[1];
        faceBayIndicator = spawnedIndicator.GetComponentsInChildren<Image>()[2];
        
    }

    public void UpdateBayIndicator(float value)
    {
        int faceNumber = 0;
        if (value > 0.33f && value <= 0.66f)
        {
            faceNumber = 1;
        }
        else if (value > 0.66f)
        {
            faceNumber = 2;
        }
        faceBayIndicator.sprite = GameController.gc.faces[faceNumber];

        Color mainColor = Color.Lerp(GameController.gc.angryColor, GameController.gc.happyColor, value);
        baseBayIndicator.color = new Color(mainColor.r * 0.7f, mainColor.g * 0.7f, mainColor.b * 0.7f);
        filledBayIndicator.color = mainColor;
        filledBayIndicator.fillAmount = value;
    }

    public void OpenBayIndicator()
    {
        baseBayIndicator.GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void CloseBayIndicator()
    {
        baseBayIndicator.GetComponent<Animator>().SetTrigger("FadeOut");
    }

    public void CarArrivalEvent()
    {
        GameController.gc.NotifyCarStationEnter(this);
    }

    public void CarDestroy()
    {
        Destroy(gameObject);
    }

}
