using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenEvent : MonoBehaviour {
	
	public Animator doorAnim;

	public void OpenDoor(){
		doorAnim.enabled = true;

	}

}
