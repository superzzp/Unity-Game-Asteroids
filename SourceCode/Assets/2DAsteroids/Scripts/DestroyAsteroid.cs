using UnityEngine;
using System.Collections;

public class DestroyAsteroid : MonoBehaviour {

	private AudioSource snd;
	private Animator anim;
	private GigaGameManager gm;
	
	public void KillAsteroid()
	{
		Destroy (gameObject);
	}

	void Start() {
		anim = GetComponent<Animator>();
		snd = GetComponent<AudioSource>();
		gm = GameObject.Find ("Game Manager").GetComponent<GigaGameManager>();
	}

	void OnMouseDown()
	{
		// Click on asteroid to destroy it
		anim.SetBool("IsExploding", true);
		snd.Play();
		gm.IncrementScore();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		anim.SetBool("IsExploding", true);
		snd.Play ();
	}

}
