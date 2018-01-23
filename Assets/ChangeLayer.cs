using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChangeLayer : MonoBehaviour {
	#if UNITY_EDITOR
	//[MenuItem ("File/ChangeLayer")]
	static void LayerChange () {
		foreach (Transform g in GameObject.Find("Level11").GetComponentsInChildren<Transform>(true)) {
			if (g.gameObject.layer == LayerMask.NameToLayer ("Water")) {
				g.gameObject.layer = LayerMask.NameToLayer ("WaveDetectable");
				Debug.Log ("Success");
			}
		}
	}
	#endif

}
