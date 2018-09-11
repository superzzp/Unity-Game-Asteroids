using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A bullet
/// </summary>
public class Bullet : MonoBehaviour {
    const float magnitude = 0.9f;
    const float totalSeconds = 1.0f;
    Timer timer;

    //apply a force to the bullet once it is fired;
    public void Initialize(Vector2 moveDirection) {
        Rigidbody2D ri2d = gameObject.GetComponent<Rigidbody2D>();
        ri2d.AddForce(moveDirection * magnitude, ForceMode2D.Impulse);
    }

    // Use this for initialization
	void Start () {
        timer = gameObject.GetComponent<Timer>();
        timer.Duration = totalSeconds;
        timer.Run();
	}
	
	// Update is called once per frame
	void Update () {
        if(timer.Finished) {
            Destroy(gameObject);
        }
	}
}
