using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnim : MonoBehaviour {

	//public Animator batAnimator;
	public GameObject endCamera;
	private GameObject bat;
	private Transform batTransform;
	public Transform batEndTransform;
	public Transform batEndSphereTransform;
	Vector3 newPosition;
	float newScale;
	static bool once, oneTimeEndSound;
	public float speed = 2.33f;
	float timeReference = 0.275f;
	float ratio; 
	float scaleReference;

	void Awake(){

		bat = GameObject.FindGameObjectWithTag ("Player");
		batTransform = bat.transform;
		once = true;
		oneTimeEndSound = true;
		
	}

	void Start(){
		ratio = Scale.instance._Scale / timeReference;
	
	}

	void OnTriggerEnter(Collider collider)
	{

		if (collider.tag == "Player" && once) 
		{
			if (GetComponent<BoxCollider> ()) {
				once = false;
				GetComponent<BoxCollider> ().enabled = false;
				Reset.instance.RemoveAllUI ();
				CameraSwitch.instance.SwitchCamera (endCamera);
				StartCoroutine ("Coroutine_AnimateBatToEnd");
			}
			else
				IntroAnim.instance.EndAnim ();
			//batAnimator.enabled = true;
			//batAnimator.SetBool ("End", true);
		}

	}

	IEnumerator Coroutine_AnimateBatToEnd(){
		batTransform.position = batEndTransform.position;
		batTransform.rotation = batEndTransform.rotation;
		Scale.instance._Scale = Scale.instance._Scale * 2;
		scaleReference = Scale.instance._Scale;
		while (Vector3.Distance (batTransform.position, batEndSphereTransform.position) > scaleReference * .05f) {
			if (Vector3.Distance (batTransform.position, batEndSphereTransform.position) < 4.75f * Scale.instance.scale) {
				if (oneTimeEndSound) {
					IntroAnim.instance.EndSound ();
					oneTimeEndSound = false;
				}
				newScale = Mathf.MoveTowards (batTransform.localScale.x, 0, Time.deltaTime * speed * ratio);
				Scale.instance._Scale = newScale;
			}
			newPosition = Vector3.MoveTowards (batTransform.position, batEndSphereTransform.position, Time.deltaTime * speed * ratio);
			batTransform.position = newPosition;
			yield return null;
		}
	
		IntroAnim.instance.EndAnim ();
	}


}
