using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnim : MonoBehaviour {

	public Transform spawnPoint;
	public GameObject introCamera;
	public BoxCollider introCollider;
	public float timeElapsed;
	private Vector3 newPosition;
	private float scaleReference, speed = 3f,newScale, timer;
	private PlayerHealth playerHealth;
	public bool finish = false;
	public ScreenFader screenFader;
	public AudioClip audioFinish;
	public GameObject bat;
	public static IntroAnim instance;
	public GameObject HUDCanvas;

	void Awake(){

		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}

		if (!introCamera) {
			introCamera = GameObject.FindGameObjectWithTag ("IntroCamera");
		}
		
		if (!introCollider) {
			if(GameObject.FindGameObjectWithTag ("IntroCollider"))
			introCollider = GameObject.FindGameObjectWithTag ("IntroCollider").GetComponent<BoxCollider> ();
		}

		if (!spawnPoint) {
			spawnPoint = GameObject.FindGameObjectWithTag ("SpawnPoint").transform;
		}
	
	}

	void Start(){
		
		if (LevelManager.levelID != 1) {
			if (introCamera) {
				StartCoroutine ("AnimatePortal");
			}
		}

		bat.GetComponent<PlayerHealth> ().enabled = false;
		foreach (BoxCollider box in bat.GetComponents<BoxCollider>()) {
			box.enabled = false;
		}

		playerHealth = bat.GetComponent<PlayerHealth> ();
		Reset.instance.ResetUI ();
		timeElapsed = 0;
		timer = 0;
		if(LevelManager.levelID != 1){
	    	Spawn ();
		}

	}

	IEnumerator AnimatePortal(){

		yield return new WaitForSeconds (2.3f);

		bat.transform.position = spawnPoint.GetChild (0).position;
		bat.transform.localRotation = spawnPoint.GetChild(0).rotation;


		scaleReference = Scale.instance._Scale;
		Scale.instance._Scale = scaleReference;
		while (Vector3.Distance (bat.transform.position, spawnPoint.position) > (0.01f * scaleReference)) {
			
			newScale = Mathf.MoveTowards (bat.transform.localScale.x, scaleReference * 2, Time.deltaTime * speed * scaleReference);
			Scale.instance._Scale = newScale;
			newPosition = Vector3.MoveTowards (bat.transform.position, spawnPoint.position, Time.deltaTime * speed * scaleReference);
			bat.transform.position = newPosition;
			yield return null;
		}
	
	}


	void Update(){
		
		timer += Time.deltaTime;
	}

	public void Spawn(){

		StartCoroutine ("WaitSpawn");
	}

	public void EndSound(){
		
		bat.GetComponent<AudioSource> ().clip = audioFinish;
		bat.GetComponent<AudioSource> ().Play ();
	}

	IEnumerator EndIntroAnim(){
		
		yield return null;

		if (HUDCanvas) {
			HUDCanvas.SetActive (true);
		}
		GetComponent<Animator> ().enabled = false;
		Reset.instance.EnableEveryUI();
		foreach (BoxCollider box in bat.GetComponents<BoxCollider>()) {
			box.enabled = true;
		}
		CameraPosition.instance.CalculateDistance ();
		if (introCollider != null) {
			introCollider.isTrigger = false;
		}
	}
		
	IEnumerator WaitSpawn(){
		
			yield return new WaitForSeconds (5f);

			CameraSwitch.instance.SwitchToMainCamera (introCamera);
			Scale.instance._Scale = Scale.instance.GetInitalScale ();

			if (spawnPoint != null) {
				bat.transform.position = spawnPoint.position;
				bat.transform.localRotation = spawnPoint.rotation;
			}

			bat.GetComponent<PlayerHealth> ().enabled = true;
			StartCoroutine ("EndIntroAnim");	
	
	}
		
	public void SpawnLevel1(){
		
		CameraSwitch.instance.SwitchToMainCamera (introCamera);
		Scale.instance._Scale = Scale.instance.GetInitalScale ();

		if (spawnPoint != null) {
			bat.transform.position = spawnPoint.position;
			bat.transform.localRotation = spawnPoint.rotation;
		}

		bat.GetComponent<PlayerHealth> ().enabled = true;
		StartCoroutine ("EndIntroAnim");

	}

	public void EndAnim(){
		Finish ();
	}

	void Finish(){
		
		timeElapsed = timer;
		finish = true;
		StartCoroutine(screenFader.FinishToBlack());


		ScoreManager.UpdateScore(
			LevelManager.levelID,
			0, //EnemyAttack.enemyCounter,
			(int)timeElapsed,
			Activation.currentRoomCounter,
			playerHealth.damageTaken()
		);

	}

}
