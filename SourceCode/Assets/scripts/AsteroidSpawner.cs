using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawn all the asteroids
/// </summary>
/// 

public class AsteroidSpawner : MonoBehaviour
{   
    //Variable
    GameObject[] prefabAsteroid;
    GameObject selectedAsteroid;

    //save the radius of the collider for preventing screen wrapping when 
    //setting the position;
    float colliderR;
    //singleton method, very interesting
    private static AsteroidSpawner Instance;

    //Property
    public static AsteroidSpawner GetInstance()
    {
        if (Instance == null)
        {
            Instance = new AsteroidSpawner();
        }
        return Instance;
    }

    // Methods
    // Use this for spawning astroids one by one, and
    // setting the initial position and moving direction of asteroids
    public void SpawnAsteroids(GameObject[] prefabs)
    {
        prefabAsteroid = prefabs;
        int randIndex = Random.Range(0, prefabs.Length);
        selectedAsteroid = prefabs[randIndex];
        colliderR = selectedAsteroid.GetComponent<CircleCollider2D>().radius;
        //make the asteroids start at just outsite the left side of the screen,move to the right
        InstantiateAsteroid(Direction.Right, new Vector3(ScreenUtils.ScreenLeft, 0, 0));
        //InstantiateAsteroid(Direction.Right, new Vector3(ScreenUtils.ScreenLeft, 0, 0));
        //start at just outside the right side of the screen,move to the left
        InstantiateAsteroid(Direction.Left, new Vector3(ScreenUtils.ScreenRight, 0, 0));
        //InstantiateAsteroid(Direction.Left, new Vector3(ScreenUtils.ScreenRight, 0, 0));
        //start at bottom, move up
        InstantiateAsteroid(Direction.Up, new Vector3(0, ScreenUtils.ScreenBottom, 0));
        //InstantiateAsteroid(Direction.Up, new Vector3(0, ScreenUtils.ScreenBottom - colliderR, 0));
        //start at top, move down
        InstantiateAsteroid(Direction.Down, new Vector3(0, ScreenUtils.ScreenTop, 0));
        //InstantiateAsteroid(Direction.Down, new Vector3(0, ScreenUtils.ScreenTop + colliderR, 0));
    }

    //Use Instantiate method make a copy of prefabAsteroid
    //This is the way to instantiate prefab at runtime
    void InstantiateAsteroid(Direction direc, Vector3 location)
    {
        GameObject asteroid = Instantiate(selectedAsteroid);
        //after instantiate the asteroid, get the script attached
        //to that asteroid and use its initialize method to set starting position
        //and apply initial force 
        asteroid.GetComponent<Asteroid>().Initialize(direc, location);
    }

}

