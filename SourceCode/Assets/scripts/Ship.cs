using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A ship
/// </summary>
public class Ship : MonoBehaviour
{


    [SerializeField]
    GameObject prefabBullet;
    //there are better methods than letting Ship to know HUD;
    //Should implement later.
    [SerializeField]
    GameObject explosionParticle;
    // game initializer
    GameInitializer gameInitializer;
    // thrust and rotation support
    Rigidbody2D rb2D;
    Vector2 thrustDirection = new Vector2(1, 0);
    const float ThrustForce = 10;
    const float RotateDegreesPerSecond = 180;

    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

    //Animator anim;                                              // Reference to the Animator component.
    //AudioSource playerAudio;                                    // Reference to the AudioSource component.
    //PlayerMovement playerMovement;                              // Reference to the player's movement.
    //PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.

	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Start()
	{
        // saved for efficiency
        gameInitializer = FindObjectOfType<GameInitializer>();
        rb2D = GetComponent<Rigidbody2D>();
        currentHealth = startingHealth;
        damaged = false;
        isDead = false;
    }
	
	/// <summary>
	/// Update is called once per frame
	/// </summary>
	void Update()
	{
        // check for rotation input
        float rotationInput = Input.GetAxis("Rotate");
        if (rotationInput != 0) {

            // calculate rotation amount and apply rotation
            float rotationAmount = RotateDegreesPerSecond * Time.deltaTime;
            if (rotationInput < 0) {
                rotationAmount *= -1;
            }
            transform.Rotate(Vector3.forward, rotationAmount);

            // change thrust direction to match ship rotation, very interesting
            float zRotation = transform.eulerAngles.z * Mathf.Deg2Rad;
            thrustDirection.x = Mathf.Cos(zRotation);
            thrustDirection.y = Mathf.Sin(zRotation);
        }

        //the state get reset each frame; It will not return true 
        //until the user has released the key and pressed it again.

        if(Input.GetKeyDown(KeyCode.Space)){
            instantiateBullet();
        }

        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;
    }

    // helper function, instanitate bullet
    //Instantiate a new bullet and set the position the same as the ship.
    void instantiateBullet(){
        GameObject bullet = Instantiate(prefabBullet);
        bullet.transform.position = transform.position;
        //get the script attached to bullet and pass in the direction
        //this ship is facing
        bullet.GetComponent<Bullet>().Initialize(thrustDirection);
    }


    /// <summary>
    /// FixedUpdate is called 50 times per second
    /// </summary>
    void FixedUpdate()
    {
        // thrust as appropriate
        // move forward
        if (Input.GetAxis("Thrust") != 0)
        {
            rb2D.AddForce(ThrustForce * thrustDirection,
                ForceMode2D.Force);
        }
        // backup the ship
        if (Input.GetAxis("BackUp") != 0)
        {
            rb2D.AddForce(-(ThrustForce * thrustDirection),
                          ForceMode2D.Force);
        }
    }

    void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // Play the hurt sound effect.
        //playerAudio.Play ();

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }
    }

    void Death ()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;
        // USE the method to let the game UI stop adding game seconds if player's health is zero
        // FindObjectOfType can find object or script from the entire scene
        gameInitializer.EndGame();
        explode();
        Destroy(gameObject);
        // Turn off any remaining shooting effects.
        //playerShooting.DisableEffects ();

        // Tell the animator that the player is dead.
        //anim.SetTrigger ("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //playerAudio.clip = deathClip;
        //playerAudio.Play ();

        // Turn off the movement and shooting scripts.
        //playerMovement.enabled = false;
        //playerShooting.enabled = false;
    }       

    // Same as the explode in class Asteroid
    // Find a way to lower coupling
    void explode()
    {
        GameObject exp = Instantiate(explosionParticle);
        exp.transform.position = gameObject.transform.position;
    }

    //Sent when an incoming collider makes contact with 
    //this object's collider (2D physics only).
    //Check if the object collide with this gameObject is 
    //tagged with "Asteroid", if yes destroy both the Asteroid
    //and the gameObject this script attach to (Ship) 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            Vector3 asteroidScale = collision.gameObject.GetComponent<Asteroid>().getCurrentScale();
            if (asteroidScale.x <= 0.5)
            {
                TakeDamage(10);
            }
            else
            {
                TakeDamage(33);
            }
            Destroy(collision.gameObject);
        }
    }
}