using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRocksFall : MonoBehaviour {

	public Animator rocksAnim;
	public GameObject camera;
	static int counter = 0;

	void Awake(){
		counter = 0;
	}


	void OnTriggerEnter(Collider c){
		if (c.tag == "Player" && counter == 0 ) {
			counter++;
			rocksAnim.enabled = true;
			CameraSwitch.instance.SwitchCamera (camera);
			Reset.instance.ResetUI ();
			gameObject.SetActive (false);
		}

	}


}
