using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class GameController : MonoBehaviour {

	//Singleton reference
	public static GameController gc;

    public GameObject[] carPrefabs;
    public GameObject bayIndicatorPrefab;
    public BayController bayController;
    public PlayerController playerController;
    public SoundManager soundManager;

    public GameObject carObj;

    public IntroController introController;

    [Header("UI Control")]
    public GameObject mainMenuCanvas;
    public GameObject gameInfoCanvas;
    public Text levelNumber;
    public Text carsServed;
    public Text carsAngeredText;
    
    public Text failedMainText;
    public Text failedHighScoreText;

    public Text difficultyText;

    [Header("Animators")]
    public Animator failedMenu;
    public Animator introAnimator;
    public Animator levelAlertAnimator;
    public Animator tutorialAnimator;

    [Header("Cameras")]
    public Camera mainMenuCamera;
    public Camera playerCamera;

    [Header("Colors & Textures")]
    public Color happyColor;
    public Color angryColor;

    public Sprite[] faces;

    int level;
    int carCount;
    int servedCarCount;
    int carsAngered;
    int totalCarsServed;
    int maxCarsAngered;
    int highScore;

    bool readyForNextLevel = false;

    Vector3 startMainMenuCameraPosition;

    public void ResetAllGameVariables()
    {
        level = 0;
        carCount = 0;
        servedCarCount = 0;
        carsAngered = 0;
        totalCarsServed = 0;
        maxCarsAngered = GetMaxCarAngered();
        highScore = 0;
    }


    public enum GameState
    {
        MAIN_MENU,
        INTRO,
        PLAYING,
        TUTORIAL,
        FAILED_SCREEN
    }
    public GameState gameState = GameState.MAIN_MENU;

    public enum Difficulty
    {
        EASY,
        NORMAL,
        HARD
    }
    public static Difficulty difficulty = Difficulty.NORMAL;


	void Start() {
        //Graphics quality control.
        if (QualitySettings.GetQualityLevel() == 0)
        {
            playerCamera.GetComponent<PostProcessingBehaviour>().profile = null;
            mainMenuCamera.GetComponent<PostProcessingBehaviour>().profile = null;
        }


        ResetAllGameVariables();
		gc = this;
        soundManager = GetComponent<SoundManager>();

        startMainMenuCameraPosition = mainMenuCamera.transform.position;
        //Ensures difficulty text is up to date.
        ChangeDifficultyDelta(0);
	}

    public void CarAngered()
    {
        carsAngered++;
        carsAngeredText.text = carsAngered.ToString() + "/" + maxCarsAngered.ToString() + " People Angered";
        if (carsAngered >= maxCarsAngered)
        {
            GameOver();
        }
    }

    public void CarServed()
    {
        carsServed.text = (++servedCarCount).ToString() + "/" + carCount.ToString() + " Cars Served";
        totalCarsServed++;
        if (servedCarCount >= carCount)
        {
            //after this car is done, the level is completed. wait for all cars to complete cleaning and move to next.
            readyForNextLevel = true;
        }
    }

    public float GetCarSecondSpacing(int level)
    {
        return (2 / (float)level) + 0.5f;
    }

    void GameOver()
    {
        gameState = GameState.FAILED_SCREEN;
        failedMenu.SetTrigger("FadeIn");
        failedMainText.text = "You reached level " + level.ToString() + " and served " + totalCarsServed.ToString() + " cars!";
        highScore = RegisterLevelHighScore();
        failedHighScoreText.text = "High Score: Level " + highScore.ToString();

        ResetAllGameVariables();
        bayController.EmptyAllBays();
    }

    void Update()
    {
        mainMenuCamera.transform.position = startMainMenuCameraPosition + new Vector3(0, 0, Input.mousePosition.x) * 0.001f;

        if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.INTRO)
        {
            FinishIntro();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && gameState == GameState.TUTORIAL)
        {
            gameState = GameState.PLAYING;
            tutorialAnimator.SetTrigger("FadeOut");
            Invoke("StartNewLevel", 2.5f);
        }

        if (gameState != GameState.PLAYING) return;
        if (Random.value < Time.deltaTime / GetCarSecondSpacing(level))
        {
            CheckCarCanSpawn(); 
        }
    }

    //Checks if a car be spawned, and if so, spawns it and sets bay information.
    void CheckCarCanSpawn()
    {
        int bayNum = bayController.FindBayToSpawn();
        if (bayNum == -1) return;

        SpawnCar(bayNum);
    }

    void SpawnCar(int bay) {
        GameObject pref = carPrefabs[Random.Range(0, carPrefabs.Length)];
        GameObject car = Instantiate(pref);
        CarInstance carInst = car.GetComponent<CarInstance>();
        bayController.AllocateBay(bay, carInst);
        carInst.thisBay = bayController.bays[bay];
    }


    public void PlayGame()
    {
        introAnimator.gameObject.SetActive(true);
        introAnimator.SetTrigger("FadeIn");
        introController.StartDialogue();
        gameState = GameState.INTRO;
    }

    public void FinishIntro()
    {

        if (gameState == GameState.PLAYING || gameState == GameState.TUTORIAL)
        {
            return;
        }

        ResetAllGameVariables();
        soundManager.Stop("SoundtrackAmbient");
        soundManager.Play("SoundtrackMain");

        introAnimator.SetTrigger("FadeOut");
        mainMenuCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

        mainMenuCanvas.SetActive(false);
        gameInfoCanvas.SetActive(true);

        gameState = GameState.TUTORIAL;
        tutorialAnimator.SetTrigger("EnableTutorial");

        playerController.transform.position = new Vector3(248.5f, 1.9f, 227.9f);
        UpdateLevelInfo(false);
    }

    //Converts a level number into a car count for this level. Sigmoid function with linear and constant functions added.
    public int LevelCarCount(int level)
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                return (2*level) + 3;
            case Difficulty.NORMAL:
                return (2*level) + 5;
            case Difficulty.HARD:
                return (3 * level) + 8;
            default:
                return (2 * level) + 5;

        }
    }

    public int GetMaxCarAngered()
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                return 7;
            case Difficulty.NORMAL:
                return 5;
            case Difficulty.HARD:
                return 3;
            default:
                return 5;
        }
    }

    public float GetBayProgressRate(int level)
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                return 0.075f + ((float)level * 0.01f);
            case Difficulty.NORMAL:
                return 0.1f + ((float)level * 0.03f);
            case Difficulty.HARD:
                return 0.125f + ((float)level * 0.05f);
            default:
                return 0.1f + ((float)level * 0.03f);
        }
    }

    public void NotifyAllEmpty()
    {
        if (!readyForNextLevel) return;
        StartNewLevel();
    }

    //Returns new high score.
    public int RegisterLevelHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            int highScore = PlayerPrefs.GetInt("HighScore");
            if (level > highScore)
            {
                PlayerPrefs.SetInt("HighScore", level);
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", level);
        }
        return PlayerPrefs.GetInt("HighScore");
    }

    public void ResetLevel()
    {
        ResetAllGameVariables();

        PlayGame();
    }

    public void StartNewLevel()
    {
        introAnimator.gameObject.SetActive(false);
        BayController.bc.EmptyAllBays();
        BayController.bc.bayProgressRate = GetBayProgressRate(level);


        level++;    
        carCount = LevelCarCount(level);
        servedCarCount = 0;
        readyForNextLevel = false;
        carsAngered = 0;
        

        levelAlertAnimator.SetTrigger("DisplayLevel");
        UpdateLevelInfo(true);

        soundManager.Play("Whoosh");
    }

    public void UpdateLevelInfo(bool enable)
    {
        if (enable)
        {
            levelNumber.gameObject.SetActive(true);
            carsAngeredText.gameObject.SetActive(true);
            carsServed.gameObject.SetActive(true);

            levelAlertAnimator.GetComponentInChildren<Text>().text = "Level " + level.ToString();
            levelNumber.text = "Level " + level.ToString();
            carsAngeredText.text = carsAngered.ToString() + "/" + maxCarsAngered.ToString() + " People Angered";

            carsServed.text = servedCarCount.ToString() + "/" + carCount.ToString() + " Cars Served";
        }
        else
        {
            levelNumber.gameObject.SetActive(false);
            carsAngeredText.gameObject.SetActive(false);
            carsServed.gameObject.SetActive(false);
        }
    }

    public void RetryButton()
    {
        failedMenu.Play("IdleOut");
        gameInfoCanvas.SetActive(false);
        soundManager.StopAll();
        soundManager.Play("SoundtrackAmbient");
        ResetLevel();
    }

    public void MainMenuButton() {
        gameState = GameState.MAIN_MENU;
        mainMenuCanvas.SetActive(true);
        gameInfoCanvas.SetActive(false);
        failedMenu.Play("IdleOut");

        mainMenuCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);

        soundManager.StopAll();
        soundManager.Play("SoundtrackAmbient");
    }

    ///Difficulty control

    public void ChangeDifficultyDelta(int amount)
    {
        int r = ((int)difficulty + amount)%3;
        int newDifficultyIndex  = r<0 ? r+3 : r;
        switch (newDifficultyIndex)
        {
            case (0):
                difficulty = Difficulty.EASY;
                break;
            case (1):
                difficulty = Difficulty.NORMAL;
                break;
            case (2):
                difficulty = Difficulty.HARD;
                break;
        }
        string upperDifficulty = difficulty.ToString();
        difficultyText.text = char.ToUpper(upperDifficulty[0]) + upperDifficulty.Substring(1).ToLower();
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
