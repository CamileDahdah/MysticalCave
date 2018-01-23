using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertSecretRoomCamera : MonoBehaviour {
	
	public GameObject secretRoomCamera;

	public void SwitchToOriginalCamera()
	{

		Reset.instance.EnableEveryUI();
		GetComponent<Animator> ().enabled = false;
		CameraSwitch.instance.SwitchToMainCamera(secretRoomCamera);
		secretRoomCamera.SetActive(false);
	}
}
