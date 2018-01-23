using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHandler : MonoBehaviour {

	public Level1Lights lightsScript;
	private float duration = 5f;
	public GameObject cameraDoor;

	// animate lights and switch camera
	public void DisableLights()
	{
		
		lightsScript.enabled = true;
		//duration of animation
		Invoke ("SwitchCamera", duration);

	}

	private void SwitchCamera()
	{

		CameraSwitch.instance.SwitchCamera (cameraDoor);
		//enable every UI
		Reset.instance.EnableEveryUI();
	}

}
