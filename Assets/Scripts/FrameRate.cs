using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameRate : MonoBehaviour {


	void Start () {
		
	}
	

	void Update () {
		
		GetComponent<Text> ().text = "Frame Rate: " + (int) (1f / Time.deltaTime);
	}
}
