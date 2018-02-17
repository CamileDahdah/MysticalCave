using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public GameObject next, previous;
	public GameObject levelsPanel, introPanel, menuPanel, settingsPanel;
	public GameObject levelsParent;
	int levelCounter, maxLevel;
	public List<Texture> levelsImages;
	public Text levelText;
	public static Menu instance;

	void Start () {
		
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}

		levelCounter = 1;
		GameManager.currentLevel = levelCounter;
		maxLevel = levelsImages.Count;
	}

	void OnEnable(){
		Time.timeScale = 1;
	}

	public void Play(){

		levelsPanel.SetActive (true);
		menuPanel.SetActive (false);
		ManageButtons ();
	}

	public void NextLevel(){
		levelCounter++;
		GameManager.currentLevel = levelCounter;
		ManageButtons ();
	

	}

	public void PreviousLevel(){
		levelCounter--;
		GameManager.currentLevel = levelCounter;
		ManageButtons ();
	}

	void ManageButtons(){

		if (levelCounter == 1) {
			previous.SetActive (false);
		}
		else {
			previous.SetActive (true);
		}

		if(levelCounter == maxLevel){
			next.SetActive (false);
		}
		else {
			next.SetActive (true);
		}

		levelsParent.GetComponent<RawImage> ().texture = levelsImages[levelCounter - 1];
		levelText.text = "" + levelCounter;
	}

	public void MenuClick(){
		
		levelsPanel.SetActive (false);
		settingsPanel.SetActive (false);
		menuPanel.SetActive (true);
	}

	public void SettingsClick(){

		settingsPanel.SetActive (true);
	}
}
