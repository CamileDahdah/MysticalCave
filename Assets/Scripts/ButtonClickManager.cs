using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickManager : MonoBehaviour {

	public AudioClip buttonClickClip, clickToStartClip;
	private AudioSource audioSource;


	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	public void ClickToStartSound(){
		audioSource.PlayOneShot (clickToStartClip);

	}

	public void NormalClickSound(){
		audioSource.PlayOneShot (buttonClickClip);

	}

}
