using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBloodJars : MonoBehaviour {

	public List<GameObject> bloodJars;
	int randNum;
	int counter;


	void Awake () {
		foreach (Transform child in gameObject.transform) {
			child.gameObject.SetActive (false);
			bloodJars.Add (child.gameObject);
		}
		InvokeRepeating ("RandomJars", 9f, 14f);

	} 

	void RandomJars(){

		if(!AvailableJars()){
			randNum = Random.Range (0, 4);
			if (bloodJars [randNum].activeSelf) {
				randNum = (randNum + 1) % 4;
			}
			bloodJars [randNum].SetActive (true);
		}

	}

	bool AvailableJars () {
		counter = 0;
		foreach (GameObject child in bloodJars) {
			if (child.activeSelf)
				counter++;
		}
		if (counter > 1)
			return true;
			else
			return false;
	}
}
