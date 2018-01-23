using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour {
	
    public GameObject panel;
	FreeMove freeMove;
	AudioSource[] allAudioSources;

	void Awake(){
		freeMove = GameObject.FindGameObjectWithTag ("Player").GetComponent<FreeMove> ();
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)){
			panel.SetActive (!panel.activeSelf);
			Pause();
		}
	}

	public void Pause(){

		Time.timeScale = Time.timeScale == 0 ? 1 : 0;

		if (Time.timeScale == 0) {
			freeMove.enabled = false;
			PauseAudioSources ();

		} else {
			freeMove.enabled = true;
			ResumeAudioSources ();
		}

  
	
    }

	public void Quit(){
		
	#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
	#else 
		Application.Quit();
	#endif

	}

	void PauseAudioSources(){
		foreach( AudioSource audioSource in allAudioSources) {
			audioSource.Pause();
		}
	}

	void ResumeAudioSources(){
		foreach(AudioSource audioSource in allAudioSources) {
			audioSource.UnPause();
		}
	}
}
