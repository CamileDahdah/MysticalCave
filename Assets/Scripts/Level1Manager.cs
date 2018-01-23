using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : MonoBehaviour {

	public GameObject endCave;
	public Light sun;
	float limitIntensity = 1f, limitBounce = 1f;
	public float speed = 1f;


	void Start () {
		StartCoroutine("DimSunLight");
	}

	IEnumerator DimSunLight(){
		yield return new WaitForSeconds (4.4f);
		while (sun.intensity > limitIntensity || sun.bounceIntensity > limitBounce) {
			sun.intensity = Mathf.MoveTowards (sun.intensity, limitIntensity, Time.deltaTime * speed);
			sun.bounceIntensity = Mathf.MoveTowards (sun.bounceIntensity, limitBounce, Time.deltaTime * speed);
			yield return null;
		}
	}
}
