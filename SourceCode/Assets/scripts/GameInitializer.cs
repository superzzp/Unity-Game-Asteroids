using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage the starting of the game
/// Spawn asteroids and set game UI
/// </summary>

public class GameInitializer : MonoBehaviour 
{   
    //2D version of prefab of the asteroid (without animations)
    //commented out for updated 2.5D visual effects
    //[SerializeField]
    //GameObject prefabAsteroid;

    //use to hold a list of new three 2.5D prefab asteroids (with animations) for games
    //Transform is similar to gameobject???
    public GameObject[] prefabs;
    [SerializeField]
    Text scoreText;
    //text to display the levels that show up on the level image
    [SerializeField]
    Text levelText;
    [SerializeField]
    GameObject levelImage;

    const float levelTime = 59.99f;
    const float startLevel = 1f;
    const float levelStartDelay = 3f;
    const float respawnSeconds = 20f;
    float currentLevel;
    float elapsedSeconds;
    float instantiationTimer;


    bool gameRunning;
    //check if during setup and prevent player from moving during the period
    bool duringSetup;
    //check if asteroids have already been spawned;
    bool asteroidSpawn;

    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
    {   
        // initialize screen utils
        ScreenUtils.Initialize();
    }

    // Use this for initialization
    void Start()
    {   
        duringSetup = true;
        gameRunning = true;
        asteroidSpawn = false;
        //set the initial level;
        currentLevel = startLevel;
        // initialize UI
        InitUI();
        instantiationTimer = levelStartDelay;

    }

    // Update is called once per frame
    void Update()
    {   
        // When game is still running, adding seconds to elapsedSeconds,
        // convert the float sec into int sec, and display the int sec on the screen
        if (gameRunning != false && duringSetup == false)
        {
            elapsedSeconds = elapsedSeconds + Time.deltaTime;
            int displaySeconds = (int)elapsedSeconds;
            scoreText.text = displaySeconds.ToString();
        }
        if(asteroidSpawn == false && duringSetup == false) {
            SpawnAsteroids();
        }
        if (CheckLevelFinished()) {
            EnterNextLevel();
        }
    }

    //helper function, called when the game need to enter next level
    //show the title picture and pause the game
    void EnterNextLevel(){
        duringSetup = true;
        currentLevel += 1f;
        setLevelText();
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
    }

    //set the level text to the current level
    void setLevelText(){
        levelText.text = "Round " + currentLevel.ToString();
    }

    //check if the game runtime is greater than specified level time
    bool CheckLevelFinished()
    {
        float elapsedSecondsInLevel = elapsedSeconds - (currentLevel - 1) * levelTime;
        if (elapsedSecondsInLevel >= levelTime) {
            return true;
        }
        return false;
    }

    void InitUI()
    {
        //set the initial content of the scoreText;
        scoreText.text = "0";
        //initialize the elapsedSeconds to 0;
        elapsedSeconds = 0;
        setLevelText();
        //hide the level image after certain seconds;
        Invoke("HideLevelImage", levelStartDelay);
    }

    // Hide the level image after game setup;
    void HideLevelImage()
    {
        levelImage.SetActive(false);
        duringSetup = false;
    }

    // spawn the asteroids when game is running
    // if it is currently level one, will spawn one set of asteroids every fix peroid of time
    // if it is currently level two, will spawn two set ...
    void SpawnAsteroids(){
        instantiationTimer -= Time.deltaTime;
        if (instantiationTimer <= 0 && gameRunning == true)
        {
            for (int i = 0; i < currentLevel; i++)
            {
                AsteroidSpawner.GetInstance().SpawnAsteroids(prefabs);
            }
            //spawn new asteroids every set seconds after the initial spawn
            instantiationTimer = respawnSeconds;
        }
    }

    // Function to end the game by setting gameRunning to false;
    // Used by the Ship script;
    // Not the best way to reduce coupling, need improvement;
    public void EndGame()
    {
        gameRunning = false;
    }


}
