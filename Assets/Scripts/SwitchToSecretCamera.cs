using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToSecretCamera : MonoBehaviour {

	public GameObject secretRoomCamera;
	static int counter, roomCounter;
	public int roomID;
	public Color secretRoomTextColor;
	public GameObject doorLeft;
	public GameObject doorRight;

	void Start()
	{

		counter = 0;
		CloseDoor ();
	}

	public void OnTriggerEnter(Collider collision)
	{

		if (collision.tag == "Player") 
		{
			counter++;

			if (counter == 1) 
			{
				gameObject.SetActive (false);
				CameraSwitch.instance.SwitchCamera (secretRoomCamera);
				transform.parent.GetComponent<Animator> ().enabled = true;
				Reset.instance.RemoveEveryUI ();
				Activation.secretRoomCounter++;
				Activation.currentRoomCounter++;
				LevelManager.instance.SetActivatedRoomList (roomID);
				PopupTextManager.instance.SetText("Secret Room Discovered\t (" 
					+ Activation.secretRoomCounter + " / "+ Activation.allRooms + ")", 6f, true, secretRoomTextColor);
			}


		}

	}

	public void AutoOpenDoor(){
		if (doorLeft != null) {
			doorLeft.transform.localEulerAngles = new Vector3 (doorLeft.transform.localEulerAngles.x, 
				-99f, doorLeft.transform.localEulerAngles.z);
			doorRight.transform.localEulerAngles = new Vector3 (doorLeft.transform.localEulerAngles.x, 
				96f, doorLeft.transform.localEulerAngles.z);
		}
	}

	public void CloseDoor(){
			if (doorLeft != null) {
			doorLeft.transform.localEulerAngles = new Vector3 (0, 0f, 0);
			doorRight.transform.localEulerAngles = new Vector3 (0, 0f, 0);
			}

	}
}
