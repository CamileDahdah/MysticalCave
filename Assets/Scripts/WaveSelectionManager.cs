using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WaveSelectionManager : MonoBehaviour{
	
    public GameObject waveEmitter;
	Slingshot slingShot;
	Text numLight, numAttack, numBounce, numNormal;
	Button buttonAttack, buttonLight, buttonBounce, buttonNormal;
	public VampireBat vampirebat;
	int oldWave;
	public Image radialImage;

    void Start(){
       
        slingShot = waveEmitter.GetComponent<Slingshot>();
		GameObject ButtonAttack = transform.Find("Button Attack").gameObject;
        GameObject ButtonLight = transform.Find("Button Light").gameObject;
        GameObject ButtonBounce = transform.Find("Button Bounce").gameObject;
        GameObject ButtonNormal = transform.Find("Button Normal").gameObject;
		buttonAttack = ButtonAttack.GetComponent<Button> ();
		buttonLight = ButtonLight.GetComponent<Button> ();
		buttonBounce = ButtonBounce.GetComponent<Button> ();
		buttonNormal = ButtonNormal.GetComponent<Button> ();
        numLight = ButtonLight.transform.GetChild(2).GetComponent<Text>();
        numAttack = ButtonAttack.transform.GetChild(2).GetComponent<Text>();
        numBounce = ButtonBounce.transform.GetChild(2).GetComponent<Text>();
        numNormal = ButtonNormal.transform.GetChild(2).GetComponent<Text>();
        UpdateText();
    }
   
    public void SelectedButton(){

        if (EventSystem.current.currentSelectedGameObject.name == "Button Normal"){

			if (buttonNormal.interactable) {
				slingShot.numWaves = 0;
			} else {
				slingShot.numWaves = -99;
			}
        }

        else if (EventSystem.current.currentSelectedGameObject.name == "Button Attack"){

			if (buttonAttack.interactable) {
				slingShot.numWaves = 1;
			} else {
				slingShot.numWaves = -99;
			}
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Button Bounce"){

			if (buttonBounce.interactable) {
				slingShot.numWaves = 2;
			} else {
				slingShot.numWaves = -99;
			}
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Button Light"){

			if (buttonLight.interactable) {
				slingShot.numWaves = 3;
			} else {
				slingShot.numWaves = -99;
			}

        }

		slingShot.SendWaveButton ();
		UpdateText();
    }

    public void UpdateText(){
		
		UpdateButtons();

        if (slingShot.currentWaves[3] < 100){
			if(slingShot.currentWaves[3] == 0)
				numLight.text = "X";
			else
           		 numLight.text = "" + slingShot.currentWaves[3];
        }
		else{
			numAttack.text = "++";
		}

        if (slingShot.currentWaves[1] < 100){
			if(slingShot.currentWaves[1] == 0)
				numAttack.text = "X";
			else
				numAttack.text = "" + slingShot.currentWaves[1];
        }
		else{
			numAttack.text = "++";
		}

        if (slingShot.currentWaves[2] < 100){
			if(slingShot.currentWaves[2] == 0)
				numBounce.text = "X";
			else
				numBounce.text = "" + slingShot.currentWaves[2];
        }
        else{
            numBounce.text = "++";
        }

        if (slingShot.currentWaves[0] < 100)
			if(slingShot.currentWaves[0] == 0)
				numNormal.text = "X";
			else
				numNormal.text = "" + slingShot.currentWaves[0];
        else
            numNormal.text = "++";
				  
    }

    void UpdateButtons(){
		//Sparkle Wave
		if (slingShot.currentWaves [0] <= 0) {
			buttonNormal.interactable = false;
		} else {
			buttonNormal.interactable = true;
		}
		//Button Attack
		if (slingShot.currentWaves [1] <= 0 || !vampirebat.isVamp) {
			buttonAttack.interactable = false;
			radialImage.gameObject.SetActive (false);
		} else if (vampirebat.isVamp && slingShot.currentWaves [1] > 0) {
			buttonAttack.interactable = true;
		}
		//Bounce Wave
		if (slingShot.currentWaves [2] <= 0) {
			buttonBounce.interactable = false;
		} else {
			buttonBounce.interactable = true;
		}
		//Light Wave
		if (slingShot.currentWaves [3] <= 0) {
			buttonLight.interactable = false;
		} else {
			buttonLight.interactable = true;
		}
    }

}