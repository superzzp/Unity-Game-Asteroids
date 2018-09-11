using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GigaGameManager : MonoBehaviour {
	public float spawnInterval = 1.0f;
	public float spawnDistance = 25f;
	public float asteroidSpeed = 2f;
	public Transform[] prefabs;
	public GameObject titleLabel;
	public GameObject instructionsLabel;
	public GameObject startButton;
	public GameObject scoreLabel;
	public bool onMenu;
	public GameObject earthLight;

	private float t = 0f;
	private List<Transform> asteroids;
	private Text scoreText;
	private int score;
	private EarthController earth;

	void Start() {
		asteroids = new List<Transform>();
		scoreText = scoreLabel.GetComponent<Text>();
		earth = GameObject.Find ("Earth").GetComponent<EarthController>();
		score = 0;
		onMenu = true;
		earthLight.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		if(!onMenu) {
			if(t < spawnInterval) t += Time.deltaTime;	
			else {
				// Spawn a new asteroid
				int index = Random.Range (0, prefabs.Length);
				float angle = Random.Range (0f, 360f);
				Vector3 targetPos = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up * spawnDistance;
				Transform obj = Instantiate(prefabs[index], targetPos, Quaternion.identity) as Transform;
				asteroids.Add(obj);
				t=0f;
			}
			// Remove any null entries that have been destroyed since the last frame
			asteroids.RemoveAll(p => p == null);
			// Cycle through each asteroid and move it forwards
			foreach(Transform asteroid in asteroids) {
				asteroid.position = Vector3.MoveTowards(asteroid.position, transform.position, asteroidSpeed * Time.deltaTime);
			}
			UpdateScore ();
		}
	}

	public void IncrementScore()
	{
		score++;
	}

	void UpdateScore()
	{
		scoreText.text = "Score: " + score.ToString();
	}
	
	public void StartGame()
	{
		// Clear down any asteroids
		asteroids.RemoveAll(p => p == null);
		foreach(Transform asteroid in asteroids) {
			Animator anim = asteroid.GetComponent<Animator>();
			AudioSource snd = asteroid.GetComponent<AudioSource>();
			snd.volume = 1f / asteroids.Count;	// Normalise volume of all asteroids
			snd.Play ();
			anim.SetBool("IsExploding", true);
		}
		asteroids.Clear();
		earth.Reset();
		score  = 0;
		titleLabel.SetActive (false);
		instructionsLabel.SetActive (false);
		startButton.SetActive (false);
		earthLight.SetActive (true);
		onMenu = false;
	}
	
	public void EndGame()
	{
		titleLabel.SetActive (true);
		instructionsLabel.SetActive (true);
		startButton.SetActive (true);
		earthLight.SetActive (false);
		onMenu = true;
	}
}
