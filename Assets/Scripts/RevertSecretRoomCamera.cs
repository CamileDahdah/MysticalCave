using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RevertSecretRoomCamera : MonoBehaviour {
	
	public GameObject secretRoomCamera;
	public NavMeshAgent navMeshAgent;

	public void SwitchToOriginalCamera(){

		Reset.instance.EnableEveryUI();
		GetComponent<Animator> ().enabled = false;
		CameraSwitch.instance.SwitchToMainCamera(secretRoomCamera);
		secretRoomCamera.SetActive(false);
		if (navMeshAgent) {
			navMeshAgent.isStopped = false;
		}

	}
}
