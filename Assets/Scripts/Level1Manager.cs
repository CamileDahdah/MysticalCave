using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Level1Manager : MonoBehaviour {

	public Light sun;
	float limitIntensity = 1f, limitBounce = 1f;
	public float speed = 1f;
	public Image fadeImage;
	Color black = new Color(0,0,0,1);
	public PlayableDirector playableDirector;
	public GameObject activation1, activation2;
	public GameObject[] junk;
	bool flag1 = true, flag2 = true;
	public Button skipVideo;
	public GameObject pauseButton;
	public Animator rocks;

	void Start () {
		
		fadeImage.color = black;
		FadeOut ();
		flag1 = true;
		flag2 = true;
		pauseButton.SetActive (false);
		skipVideo.gameObject.SetActive (true);

		skipVideo.onClick.AddListener( delegate() {
			FadeInOutEnd();

		});
	}

	void Update(){
		
		if (flag1 && activation1.activeSelf) {
			FadeInOut ();
			activation1.SetActive (false);
			flag1 = false;
		}

		if (flag2 && activation2.activeSelf) {
			FadeInOutEnd ();
			activation2.SetActive (false);
			flag2 = false;
		}
			
	}


	public void FadeOut(){

		StopAllCoroutines();
		StartCoroutine ("FadeOutEnum");


	}

	public void FadeIn(){
		StopAllCoroutines();
		StartCoroutine ("FadeInEnum");


	}

	void FadeInOut(){
		StartCoroutine ("FadeInOutEnum");

	}

	IEnumerator FadeOutEnum(){

		while (fadeImage.color.a > 0) {
			fadeImage.color = new Color (0 ,0 ,0 , Mathf.MoveTowards (fadeImage.color.a, 0, Time.deltaTime/2));
			yield return null;
		}

		playableDirector.Play ();

	}

	IEnumerator FadeInEnum(){

		while (fadeImage.color.a < 1) {
			fadeImage.color = new Color (0 ,0 ,0 , Mathf.MoveTowards (fadeImage.color.a, 1, Time.deltaTime/2));
			yield return null;
		}
	}

	IEnumerator FadeInOutEnum(){

		while (fadeImage.color.a < 1) {
			fadeImage.color = new Color (0 ,0 ,0 , Mathf.MoveTowards (fadeImage.color.a, 1, Time.deltaTime/2));
			yield return null;
		}


		while (fadeImage.color.a > 0) {
			fadeImage.color = new Color (0 ,0 ,0 , Mathf.MoveTowards (fadeImage.color.a, 0, Time.deltaTime/2));
			yield return null;
		}

	}

	void FadeInOutEnd(){
		skipVideo.gameObject.SetActive (false);
		StopAllCoroutines ();
		StartCoroutine ("FadeInOutEndEnum");

	}

	IEnumerator FadeInOutEndEnum(){

		sun.intensity = limitIntensity;
		sun.bounceIntensity = limitBounce;

		while (fadeImage.color.a < 1) {

			fadeImage.color = new Color (0 ,0 ,0 , Mathf.MoveTowards (fadeImage.color.a, 1, Time.deltaTime/2));
			yield return null;
		}

		foreach (GameObject junky in junk) {
			Destroy (junky);
		}

		IntroAnim.instance.SpawnLevel1 ();
		RocksFallingEndAnim.instance.EndAnimQuick ();
		while (fadeImage.color.a > 0) {
			fadeImage.color = new Color (0 ,0 ,0 , Mathf.MoveTowards (fadeImage.color.a, 0, Time.deltaTime / 2 ));
			yield return null;
		}

		pauseButton.SetActive (true);

	}

}
