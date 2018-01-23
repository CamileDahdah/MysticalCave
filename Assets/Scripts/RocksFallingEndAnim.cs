using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksFallingEndAnim : MonoBehaviour {

	public GameObject camera;
	public static RocksFallingEndAnim instance;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}
	}

	public void EndAnim(){

		Invoke ("EndAnimWait", 1.5f);
	}

	void EndAnimWait() {
		CameraSwitch.instance.SwitchToMainCamera (camera);
		Reset.instance.EnableEveryUI ();
	}
}
