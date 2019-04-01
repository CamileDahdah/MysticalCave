using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Tutorial: MonoBehaviour{

	public GameObject joystickZone;
	public Text speechText;
	public GameObject tutorialPanel, speechPanel;
	public enum TutorialLevels { Level1 = 1, level3 = 3 };
	public const string TUTORIAL_STRING = "tutorialPhase";
	public static Tutorial instance = null;
	const float PANEL_ALPHA_LIMIT = 114 / 255f;
	string[] speechList;
	int currentSpeechCounter, speechLength;
	private Tutorial (){}


	void Awake(){
		
		if (LevelManager.instance.GetLevelId () == (int) TutorialLevels.Level1) {
			TextAsset textAsset = Resources.Load<TextAsset> ("Tutorial/Level1");
			string allText = textAsset.text;
			speechList = allText.Split (new string[] { "\n" }, StringSplitOptions.None);
			currentSpeechCounter = 0;
			speechLength = speechList.Length;

		}

	}

	public static Tutorial GetInstance(){

		if (instance == null) {
			instance = new Tutorial();
		}

		return instance;
	}

	public int GetPhase (){
		return PlayerPrefs.GetInt (TUTORIAL_STRING, (int) TutorialLevels.Level1);
	}

	public void SetPhase (int id){
		PlayerPrefs.SetInt (TUTORIAL_STRING, id);
		PlayerPrefs.Save ();
	}

	void EnableSpeechPanel (){
		
		speechPanel.GetComponent<Image> ().color = new Color (0, 0, 0); 
		speechPanel.SetActive (true);
		StopCoroutine ("AnimatePanelAlpha");
		StartCoroutine ("AnimatePanelAlpha", PANEL_ALPHA_LIMIT);

	}

	private IEnumerator AnimatePanelAlpha (float targetValue){

		Image image = speechPanel.GetComponent<Image> ();

		while (image.color.a < targetValue) {
			image.color = new Color (0, 0, 0, Mathf.MoveTowards (image.color.a, targetValue, Time.deltaTime));
			yield return null;
		}

	}

	private IEnumerator AnimatePadArea (float targetValue){

		Image image = joystickZone.GetComponent<Image> ();

		while (image.color.a < targetValue) {

			image.color = new Color (0.5f, 0.5f, 0.5f, Mathf.PingPong (image.color.a, targetValue));
			yield return null;
		}

	}

}
