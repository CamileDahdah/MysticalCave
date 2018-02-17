using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {
	
    public GameObject panel;
	FreeMove freeMove;
	AudioSource[] allAudioSources;
	public GameObject leftHand, rightHand;
	public Sprite activeHand, passiveHand;

	void Awake(){
		freeMove = GameObject.FindGameObjectWithTag ("Player").GetComponent<FreeMove> ();
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		leftHand.GetComponent<Button>().onClick.AddListener(delegate() {
			FlipUI("left");
		});
		rightHand.GetComponent<Button>().onClick.AddListener(delegate() {
			FlipUI("right");
		});
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

	void FlipUI(string direction){
		
		if (direction == "right") {
			
			rightHand.GetComponent<Image> ().sprite = activeHand;
			leftHand.GetComponent<Image> ().sprite = passiveHand;
			rightHand.GetComponent<RectTransform> ().localScale = new Vector3 (-1, 1, 1);
			leftHand.GetComponent<RectTransform> ().localScale = new Vector3 (-1, 1, 1);

		} else if(direction == "left"){
			
			rightHand.GetComponent<Image> ().sprite = passiveHand;
			leftHand.GetComponent<Image> ().sprite = activeHand;
			rightHand.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
			leftHand.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		}
	}
}
