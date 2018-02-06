using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public GameObject next, previous;
	public GameObject levelsPanel, introPanel, menuPanel;
	public GameObject levelsParent;
	int levelCounter, maxLevel;
	public List<Texture> levelsImages;
	public Text levelText;

	void Start () {
		levelCounter = 1;
		GameManager.currentLevel = levelCounter;
		maxLevel = levelsImages.Count;
	}

	public void Play(){

		levelsPanel.SetActive (true);
		introPanel.SetActive (false);
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
		introPanel.SetActive (true);
		menuPanel.SetActive (true);
	}
}
