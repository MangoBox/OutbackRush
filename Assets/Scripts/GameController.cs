using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	//Singleton reference
	public static GameController gc;

    public GameObject[] carPrefabs;
    public GameObject bayIndicatorPrefab;
    public BayController bayController;

    public List<CarInstance> allWorldCars = new List<CarInstance>();

    public GameObject carObj;

    public IntroController introController;

    [Header("UI Control")]
    public GameObject mainMenuCanvas;

    [Header("Animators")]
    public Animator introAnimator;

    [Header("Cameras")]
    public Camera mainMenuCamera;
    public Camera playerCamera;

    [Header("Colors & Textures")]
    public Color happyColor;
    public Color angryColor;

    public Sprite[] faces;


    public enum GameState
    {
        MAIN_MENU,
        INTRO,
        PLAYING,
    }
    public GameState gameState = GameState.MAIN_MENU;

	void Awake() {
		gc = this;
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

    void Update()
    {
        if (gameState != GameState.PLAYING) return;
        if (Random.value < (1f / 60f))
        {
            CheckCar();
        }

        foreach (CarInstance ci in allWorldCars)
        {
            ci.UpdateBayIndicator(bayController.bays[ci.bayNum].currentProgress);
        }
    }

    void SpawnCar(int bay) {
        GameObject pref = carPrefabs[Random.Range(0, carPrefabs.Length)];
        GameObject car = Instantiate(pref);
        CarInstance carInst = car.GetComponent<CarInstance>();
        carInst.SetAnimationBay(bay);
        carInst.bayNum = bay;
        bayController.bays[bay].currentProgress = 1f;
        allWorldCars.Add(carInst);
    }

    public void NotifyCarReady(CarInstance carInst) {
        carInst.CarLeave();
        bayController.bays[carInst.bayNum].carStatus = Bay.CarStatus.EMPTY;
    }

    public void NotifyCarStationEnter(CarInstance carInst)
    {
        bayController.bays[carInst.bayNum].currentProgress = 1f;
        
    }

    public CarInstance GetCarByBay(int bay)
    {
        return allWorldCars.Find(c => c.bayNum == bay);
    }

    public void PlayGame()
    {
        introAnimator.SetTrigger("FadeIn");
        introController.StartDialogue();
        gameState = GameState.INTRO;
    }

    public void FinishIntro()
    {
        introAnimator.SetTrigger("FadeOut");
        mainMenuCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

        mainMenuCanvas.SetActive(false);

        gameState = GameState.PLAYING;
    }

    //Converts a level number into a car count for this level. Sigmoid function with linear and constant functions added.
    public int LevelCarCount(int level)
    {
        return Mathf.FloorToInt((29.2f / 1 + Mathf.Exp(-0.2f * ((float)level - 5.5f))) - 3.8f + (0.6f * (float)level));
    }

}
