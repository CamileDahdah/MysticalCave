using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksFallingEndAnim : MonoBehaviour {

	public GameObject camera;
	public static RocksFallingEndAnim instance;
	Animator animator;

	void Awake(){
		
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}

		animator = GetComponent<Animator> ();
	}

	public void EndAnim(){

		Invoke ("EndAnimWait", 1.5f);
	}

	void EndAnimWait() {
		CameraSwitch.instance.SwitchToMainCamera (camera);
		Reset.instance.EnableEveryUI ();
	}

	public void EndAnimQuick(){

		animator.Play ("New State", 0, 1f);
	}
}
