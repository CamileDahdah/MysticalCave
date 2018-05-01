using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Level1Manager : MonoBehaviour {

	public GameObject endCave;
	public Light sun;
	float limitIntensity = 1f, limitBounce = 1f;
	public float speed = 1f;
	public Image fadeImage;
	Color black = new Color(0,0,0,1);
	public PlayableDirector playableDirector;
	public GameObject activation1, activation2;
	public GameObject[] junk;
	bool flag1 = true, flag2 = true;

	void Start () {
		StartCoroutine("DimSunLight");
		fadeImage.color = black;
		FadeOut ();
		flag1 = true;
		flag2 = true;
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

	IEnumerator DimSunLight(){
		yield return new WaitForSeconds (4.4f);
		while (sun.intensity > limitIntensity || sun.bounceIntensity > limitBounce) {
			sun.intensity = Mathf.MoveTowards (sun.intensity, limitIntensity, Time.deltaTime * speed);
			sun.bounceIntensity = Mathf.MoveTowards (sun.bounceIntensity, limitBounce, Time.deltaTime * speed);
			yield return null;
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
		StartCoroutine ("FadeInOutEndEnum");

	}

	IEnumerator FadeInOutEndEnum(){

		while (fadeImage.color.a < 1) {
			fadeImage.color = new Color (0 ,0 ,0 , Mathf.MoveTowards (fadeImage.color.a, 1, Time.deltaTime/2));
			yield return null;
		}

		foreach (GameObject junky in junk) {
			Destroy (junky);
		}

		IntroAnim.instance.SpawnLevel1 ();

		while (fadeImage.color.a > 0) {
			fadeImage.color = new Color (0 ,0 ,0 , Mathf.MoveTowards (fadeImage.color.a, 0, Time.deltaTime / 2 ));
			yield return null;
		}



	}

}
