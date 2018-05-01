using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldPanelManager : MonoBehaviour {

	public List<GameObject> WorldlList;

	public static WorldPanelManager instance;
	int currentWorld;
	public GameObject leftArrow, rightArrow;
	public Sprite highlightedBullet, disabledBullet;
	public List<GameObject> bullets;
	public List<GameObject> worlds;
	int bigSize = 15;
	int smallSize = 12;
	public GameObject lockGameObject, clickWorld;


	void Awake () {
		
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}

		currentWorld = 0;
	}
	

	void OnEnable(){

		UpdateWorlds ();
		UpdateArrows ();

	}

	void UpdateWorlds(){

		for (int i = 0; i < bullets.Count; i++) {
			
			if (currentWorld == i) {
				worlds [i].SetActive (true);
				bullets [i].GetComponent<Image> ().sprite = highlightedBullet;
				bullets [i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (bigSize, bigSize);

			} else {
				worlds [i].SetActive (false);
				bullets [i].GetComponent<Image> ().sprite = disabledBullet;
				bullets [i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (smallSize, smallSize);
			}

		}

	}

	void UpdateArrows(){
		
		if (currentWorld == 0) {
			leftArrow.SetActive (false);
			rightArrow.SetActive (true);
			lockGameObject.SetActive (false);
			clickWorld.SetActive (true);

		} else {
			leftArrow.SetActive (true);
			rightArrow.SetActive (false);
			lockGameObject.SetActive (true);
			clickWorld.SetActive (false);
		}
			
	}

	public void NextArrowClick(){

		++currentWorld;
		UpdateWorlds ();
		UpdateArrows ();
	}

	public void PreviousArrowClick(){

		--currentWorld;
		UpdateWorlds ();
		UpdateArrows ();
	}


}
