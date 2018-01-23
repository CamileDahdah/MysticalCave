using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {
	
	public GameObject doorCamera, tutorialDoor;
	float openDoorDuration = 1.5f;
	float duration = .2f;
	float count;

	void Awkae(){
		count = 0;
	}

	void OnTriggerEnter(Collider other){
		
		if (other.gameObject.tag == "Player" && count == 0){
			TouchManager.instance.joyStick.SetActive(false);
			Invoke("AnimateEverything", duration);
			transform.parent.gameObject.SetActive (false);
			count++;
		}
	}

	private void AnimateDoor(){
		tutorialDoor.GetComponent<Animator>().enabled = true;
	}

	private void AnimateEverything(){
		//remove every UI
		Reset.instance.ResetUI();
		//animate Door
		Invoke("AnimateDoor", openDoorDuration);

		//STOP SCRIPT
		this.enabled = false;
		//switch camera
		CameraSwitch.instance.SwitchCamera(doorCamera);

	}		
}
