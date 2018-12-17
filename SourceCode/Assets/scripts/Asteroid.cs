using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An asteroid
/// </summary>
public class Asteroid : MonoBehaviour {
    
    //[SerializeField]
    //Sprite hard;
    //[SerializeField]
    //Sprite lava;
    //[SerializeField]
    //Sprite ore;
    //field used to store the explosion particle of asteroid
    [SerializeField]
    GameObject explosionParticle;
    AudioSource aus;
    Vector3 currentScale;

    // specify what is in the gold, red and purple for readability, 
    // do not have to specify but doing so is recommended.
    void Awake()
    {   
        //hard = Resources.Load<Sprite>("sprites/hard");
        //lava = Resources.Load<Sprite>("sprites/lava");
        //ore = Resources.Load<Sprite>("sprites/ore");
        //localScale: The scale of the transform relative to the parent. 
        currentScale = transform.localScale;
        aus = GetComponent<AudioSource>();
    }

    public void Initialize(Direction direc, Vector3 location) {
        
        Direction direction = direc;
        Vector3 initialLocation = location;

        //set the initial position for the asteroid base on the location input
        //?why after adding GameObject, the transform does not work?
        transform.position = initialLocation;

        //set the audio source of explosion


        //ramdomly select one of the three sprites;
        //comment out for updating the three 2.5D animation prefabs
        //int spriteNum = Random.Range(0, 3);
        //SpriteRenderer spriteR = FindObjectOfType<SpriteRenderer>();
        //if (spriteNum == 0)
        //{
        //    spriteR.sprite = hard;
        //}
        //if (spriteNum == 1)
        //{
        //    //purple = (Sprite)Resources.Load("purple");
        //    spriteR.sprite = ore;
        //}
        //if (spriteNum == 2)
        //{
        //    //red = (Sprite)Resources.Load("red");
        //    spriteR.sprite = lava;
        //}

        //choose the direction that start going base on
        //one of the four input direction;
        float randomAngle = Random.Range(0, Mathf.PI/6);
        if (direction == Direction.Up){
            randomAngle = 75 * Mathf.Deg2Rad + randomAngle;
        }else if (direction == Direction.Left){
            randomAngle = 165 * Mathf.Deg2Rad + randomAngle;
        }else if (direction == Direction.Down){
            randomAngle = 255 * Mathf.Deg2Rad + randomAngle;
            //one more else if for readability
        }else if (direction == Direction.Right){
            randomAngle = 345 * Mathf.Deg2Rad + randomAngle;
        }
        StartMoving(randomAngle);

    }

    //get the current scale in relation to its parents
    public Vector3 getCurrentScale(){
        return currentScale;
    }

    //helper method, move the gameObject based on the input angle
    //and a randomly generated force
    void StartMoving(float angle){
        Vector2 moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        //add an random inpulse force within the predefined range;
        const float minImpulseForce = 1f;
        const float maxImpulseForce = 2f;
        float magnitude = Random.Range(minImpulseForce, maxImpulseForce);
        // do not use FindObjectOfType here because it is a scene wide scearch,
        // which may return the Rigidbody 2D of another game objects
        // instead, use GetComponent!
        Rigidbody2D ri2d = gameObject.GetComponent<Rigidbody2D>();
        ri2d.AddForce(moveDirection * magnitude, ForceMode2D.Impulse);
    }

    //use the explosionParticle to instantiate explosion effects for asteroid;
    void explode(){
        aus.Play();
        GameObject exp = Instantiate(explosionParticle);
        exp.transform.position = gameObject.transform.position;
    }

    //Sent when an incoming collider makes contact with 
    //this object's collider (2D physics only).
    //Check if the object collide with this gameObject is 
    //tagged with "Bullet", if yes destroy the Bullet
    //and cut the scale and the collider of gameObject this script attach to(Asteroid)
    //in half.
    void OnCollisionEnter2D(Collision2D collision)
    {   //get the info about colliding gameObject
        if (collision.gameObject.tag == "Bullet") {

            currentScale = new Vector3(currentScale.x / 2, currentScale.y / 2, currentScale.z / 2);
            transform.localScale = currentScale;
            CircleCollider2D circleCollider = gameObject.GetComponent<CircleCollider2D>();
            float currentRadius = circleCollider.radius;
            circleCollider.radius = currentRadius / 2;
            Destroy(collision.gameObject);
            //destroy the asteroid if it is smaller than half of the original scale
            //(if the asteroid has already been split once)
            //the original scale is currently set as 0.6
            if (currentScale.x < 0.3)
            {
                explode();
                Destroy(gameObject);
            }
            else
            {
                explode();
                GameObject asteroid1 = Instantiate(gameObject, transform.position, Quaternion.identity) as GameObject;
                GameObject asteroid2 = Instantiate(gameObject, transform.position, Quaternion.identity) as GameObject;
                Destroy(gameObject);
                //generate a random angle for the StartMoving method
                //to move the asteroids
                float randomAngle1 = Random.Range(0, 2 * Mathf.PI);
                float randomAngle2 = Random.Range(0, 2 * Mathf.PI);
                //float randomAngle3 = Random.Range(0, 2 * Mathf.PI);
                asteroid1.GetComponent<Asteroid>().StartMoving(randomAngle1);
                asteroid2.GetComponent<Asteroid>().StartMoving(randomAngle2);
            }
        }
        if (collision.gameObject.tag == "Ship") {
            explode();
            Destroy(gameObject);
        }
    }

    void Start () {
        //ParticleSystem explosion = explosionParticle.GetComponent<ParticleSystem>();
    }

}
