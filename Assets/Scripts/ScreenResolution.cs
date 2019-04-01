using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class ScreenResolution : MonoBehaviour {
	
	public List<GameObject> resolutionButtons = new List<GameObject>();
	public List<GameObject> resolutionTexts = new List<GameObject>();
	public List<GameObject> checkMarks = new List<GameObject>();
	public Sprite selectedSprite, deselectedSprite;
	int selectedIndex, firstIndex;
	public GameObject applyButton;
	public GameObject menuButton;

//	int[][] resolutions2D = {
//		new int[]{ 960, 540 },
//		new int[]{ 1280, 720 },
//		new int[]{ 1920, 1080 },
//		new int[]{ 2560, 1440 }
//	};


	public enum qualityEnum 
	{
		
		 full = 3,  high = 2 , medium = 1 , low = 0 

	};

	void Awake(){


		int index = (int) qualityEnum.low;

		foreach(GameObject button in resolutionButtons){
			
			int current = index++;

			button.GetComponent<Button> ().onClick.AddListener (delegate() {
				SetQuality (current);

			});

		}

		applyButton.GetComponent<Button> ().onClick.AddListener (delegate() {
			ApplyChanges ();

		});

		menuButton.GetComponent<Button> ().onClick.AddListener (delegate() {
			GoToMainMenu ();

		});
	}

	void OnEnable(){
		
		selectedIndex = PlayerPrefs.GetInt("qualitySettings", (int) qualityEnum.high);

		firstIndex = selectedIndex;

		SetQuality (selectedIndex);

	}


	public void SetQuality(int newIndex) {
	

			DeslectButton (selectedIndex);
			SelectButton (newIndex);
			QualitySettings.SetQualityLevel (newIndex);
			//Screen.SetResolution (resolutions2D [newIndex] [0], resolutions2D [newIndex] [1], true);

			selectedIndex = newIndex;

    }

	void DeslectButton (int selectedIndex){
		
		resolutionButtons[selectedIndex].GetComponent<Image>().sprite = deselectedSprite ;
		resolutionTexts [selectedIndex].GetComponent<Gradient> ().enabled = false;
		checkMarks[selectedIndex].SetActive(false);

	}

	void SelectButton(int newIndex){
		
		resolutionButtons[newIndex].GetComponent<Image>().sprite = selectedSprite ;
		resolutionTexts [newIndex].GetComponent<Gradient> ().enabled = true;
		checkMarks[newIndex].SetActive(true);

	}

	void ApplyChanges(){
		
		PlayerPrefs.SetInt("qualitySettings", selectedIndex);
		PlayerPrefs.Save ();
		Menu.instance.HomeButtonClick ();

	}

	void GoToMainMenu (){

		Menu.instance.SettingsToMenu ();
		SetQuality (firstIndex);

	}

}
