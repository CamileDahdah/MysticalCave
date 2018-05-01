using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WaveSelectionManager : MonoBehaviour{
	
    public GameObject waveEmitter;
	Slingshot slingShot;
	Text numLight, numBounce, numSparkles;
	Button buttonAttack, buttonLight, buttonBounce, buttonSparkles;
	Image imageAttack, imageLight, imageBounce, imageSparkles;
	public VampireBat vampirebat;
	int oldWave;
	Color disabledImageColor = new Color(0f,0f,0f,1f);
	Color enabledImageColor = new Color(1f,1f,1f,1f);

    void Start(){
       
        slingShot = waveEmitter.GetComponent<Slingshot>();
		GameObject ButtonAttack = transform.Find("Button Attack").gameObject;
		GameObject ButtonLight = transform.Find("Button Light").gameObject;
		GameObject ButtonBounce = transform.Find("Button Bounce").gameObject;
		GameObject ButtonSparkles = transform.Find("Button Sparkles").gameObject;
		buttonAttack = ButtonAttack.transform.GetChild(0).GetComponent<Button> ();
		buttonLight = ButtonLight.transform.GetChild(0).GetComponent<Button> ();
		buttonBounce = ButtonBounce.transform.GetChild(0).GetComponent<Button> ();
		buttonSparkles = ButtonSparkles.transform.GetChild(0).GetComponent<Button> ();
		imageAttack = ButtonAttack.transform.GetChild(2).GetComponent<Image> ();
		imageLight = ButtonLight.transform.GetChild(1).GetComponent<Image> ();
		imageBounce = ButtonBounce.transform.GetChild(1).GetComponent<Image> ();;
		imageSparkles = ButtonSparkles.transform.GetChild(1).GetComponent<Image> ();;
        numLight = ButtonLight.transform.GetChild(2).GetComponent<Text>();
        numBounce = ButtonBounce.transform.GetChild(2).GetComponent<Text>();
		numSparkles = ButtonSparkles.transform.GetChild(2).GetComponent<Text>();
        UpdateText();
    }
   
    public void SelectedButton(){

		switch (EventSystem.current.currentSelectedGameObject.transform.parent.name) {

		case ("Button Sparkles"):

			if (buttonSparkles) {
				slingShot.numWaves = 0;
			} else {
				slingShot.numWaves = -99;
			}

			break;

		case ("Button Bounce"):

			if (buttonBounce) {
				slingShot.numWaves = 2;
			} else {
				slingShot.numWaves = -99;
			}

			break;

		case ("Button Attack"):

			if (buttonAttack) {
				slingShot.numWaves = 1;
			} else {
				slingShot.numWaves = -99;
			}

			break;

		case ("Button Light"):

			if (buttonLight) {
				slingShot.numWaves = 3;
			} else {
				slingShot.numWaves = -99;
			}

			break;

		}

		slingShot.SendWaveButton ();
		UpdateText();
    }

    public void UpdateText(){
		
		UpdateButtons();

        if (slingShot.currentWaves[3] < 100){
			if(slingShot.currentWaves[3] <= 0)
				numLight.text = "";
			else
           		 numLight.text = "" + slingShot.currentWaves[3];
        }
		else{
			numLight.text = "++";
		}

        if (slingShot.currentWaves[2] < 100){
			if(slingShot.currentWaves[2] <= 0)
				numBounce.text = "";
			else
				numBounce.text = "" + slingShot.currentWaves[2];
        }
        else{
            numBounce.text = "++";
        }

        if (slingShot.currentWaves[0] < 100)
			if(slingShot.currentWaves[0] <= 0)
				numSparkles.text = "";
			else
				numSparkles.text = "" + slingShot.currentWaves[0];
        else
            numSparkles.text = "++";
				  
    }

    void UpdateButtons(){
		
		//Sparkle Wave
		if (slingShot.currentWaves [0] <= 0) {
			buttonSparkles.gameObject.SetActive(false);
			imageSparkles.color = disabledImageColor;
		} else {
			buttonSparkles.gameObject.SetActive(true);
			imageSparkles.color = enabledImageColor;
		}

		//Button Attack
		if ( !vampirebat.isVamp ) {
			buttonAttack.gameObject.SetActive(false);
			imageAttack.color = disabledImageColor;

		} else if (vampirebat.isVamp) {
			buttonAttack.gameObject.SetActive(true);
			imageAttack.color = enabledImageColor;
		}

		//Bounce Wave
		if (slingShot.currentWaves [2] <= 0) {
			buttonBounce.gameObject.SetActive(false);
			imageBounce.color = disabledImageColor;
		} else {
			buttonBounce.gameObject.SetActive(true);
			imageBounce.color = enabledImageColor;
		}

		//Light Wave
		if (slingShot.currentWaves [3] <= 0) {
			buttonLight.gameObject.SetActive(false);
			imageLight.color = disabledImageColor;
		} else {
			buttonLight.gameObject.SetActive(true);
			imageLight.color = enabledImageColor;
		}
    }

}