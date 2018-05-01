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
	string currentUIDirection;

	void Start(){

		freeMove = GameObject.FindGameObjectWithTag ("Player").GetComponent<FreeMove> ();
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

		leftHand.GetComponent<Button>().onClick.AddListener(delegate() {
			PlayerPrefs.SetString ("UIDirection", "left");
			PlayerPrefs.Save();
			FlipUI("left");
		});
		rightHand.GetComponent<Button>().onClick.AddListener(delegate() {
			PlayerPrefs.SetString ("UIDirection", "right");
			PlayerPrefs.Save();
			FlipUI("right");
		});
			
		FlipUI (PlayerPrefs.GetString ("UIDirection", "right"));
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
			if (freeMove) {
				freeMove.enabled = false;
			}
			PauseAudioSources ();
			Reset.instance.PauseUI ();

		} else {
			if (freeMove) {
				freeMove.enabled = true;
			}
			ResumeAudioSources ();
			Reset.instance.EnableResumeUI ();
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
		foreach(AudioSource audioSource in allAudioSources) {
			if (audioSource) {
				audioSource.Pause ();
			}
		}
	}

	void ResumeAudioSources(){
		foreach(AudioSource audioSource in allAudioSources) {
			if (audioSource) {
				audioSource.UnPause ();
			}
		}
	}

	void FlipUI(string direction){

		if (direction != currentUIDirection) {
			
			//by default direction to the right
			if (direction == "right") {
			
				rightHand.GetComponent<Image> ().sprite = activeHand;
				leftHand.GetComponent<Image> ().sprite = passiveHand;
				rightHand.GetComponent<RectTransform> ().localScale = new Vector3 (-1, 1, 1);
				leftHand.GetComponent<RectTransform> ().localScale = new Vector3 (-1, 1, 1);
				Reset.instance.Flip ("right");

			} else if (direction == "left") {
			
				rightHand.GetComponent<Image> ().sprite = passiveHand;
				leftHand.GetComponent<Image> ().sprite = activeHand;
				rightHand.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
				leftHand.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
				Reset.instance.Flip ("left");
			}

			currentUIDirection = direction;
		}

	}
}
