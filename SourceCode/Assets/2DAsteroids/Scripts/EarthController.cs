using UnityEngine;
using System.Collections;

public class EarthController : MonoBehaviour {
	public float warningDistance = 5f;
	public float maxVolume = 0.5f;

	private AudioSource siren;
	private GigaGameManager gm;
	private Animator anim;

	void Start() {
		anim = GetComponent<Animator>();
		siren = GetComponent<AudioSource>();
		gm = GameObject.Find ("Game Manager").GetComponent<GigaGameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		// Check if any asteroids in warning range
		if(!gm.onMenu) {
			Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, warningDistance);
			if(hits.Length > 1) FadeIn();
			else FadeOut ();
		}
		else FadeOut ();
	}

	public void Reset()
	{
		anim.SetBool("IsDead", false);
	}

	void OnTriggerEnter2D(Collider2D hit)
	{
		if(hit.gameObject.name.Contains("Asteroid")) {
			// We are hit
			anim.SetBool ("IsDead", true);
			gm.EndGame();
		}
	}

	// Fade in Siren
	void FadeIn() {
		if(siren.volume < maxVolume) {
			siren.volume += 0.2f * Time.deltaTime;
		}
	}

	// Fade out Siren
	void FadeOut() {
		if(siren.volume > 0.01f) {
			siren.volume -= 0.5f * Time.deltaTime;
		}
	}
}
