using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public GameObject next, previous;
	public GameObject levelsPanel, menuPanel, settingsPanel, worldPanel, skillsPanel;
	public GameObject levelsParent, worldCanvas;
	int levelCounter, maxLevel;
	public List<Texture> levelsImages;
	public Text levelText;
	public static Menu instance;
	public Text worldScore;
	int num = 0;

	void Start () {
		
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}

		levelCounter = 1;
		GameManager.currentLevel = levelCounter;
		maxLevel = levelsImages.Count;
		SetWorldScore ();
	}

	void OnEnable(){
		Time.timeScale = 1;
	}

	public void Play(){

		worldPanel.SetActive (true);
		menuPanel.SetActive (false);
		worldCanvas.SetActive (true);

	}

	public void WorldClick(){

		worldPanel.SetActive (false);
		levelsPanel.SetActive (true);
		ManageButtons ();
		worldCanvas.SetActive (false);
	}

	public void HomeButtonClick(){
		worldPanel.SetActive (false);
		menuPanel.SetActive (true);
		worldCanvas.SetActive (false);
		skillsPanel.SetActive (false);
		settingsPanel.SetActive (false);
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

		worldCanvas.SetActive (true);
		worldPanel.SetActive (true);
		levelsPanel.SetActive (false);
	}

	public void SettingsClick(){
		settingsPanel.SetActive (true);
	}

	public void SkillsClick(){
		skillsPanel.SetActive (true);
		menuPanel.SetActive (false);
	}

	public void SettingsToMenu(){

		menuPanel.SetActive (true);
		settingsPanel.SetActive (false);
	}

	public void SetWorldScore(){

		int num = GameManager.Instance.GetWorldScore ();

		worldScore.text = num + "/18";
	}

}
