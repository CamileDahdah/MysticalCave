using UnityEngine;




public class Reset :MonoBehaviour{
  

    public GameObject baat;
    public GameObject joyStick, waveSelection, joyStickZone;
    public TouchManager touchManager;
    public FreeMove freeMove;
    public GameObject pauseButton;
    public static Reset instance = null;
	public GameObject mainPanel;


    void Awake(){
  
		if (instance == null) {
			instance = this;

		}else {
            Destroy(gameObject);
		}
    }

	//remove every UI excluding pause button
    public void ResetUI() {

        touchManager.enabled = false;      
		freeMove.enabled = false; 
		joyStick.SetActive(false); 	
		waveSelection.SetActive(false);

    }

	//remove every UI including pause button
	public void PauseUI() {

		touchManager.enabled = false;      
		freeMove.enabled = false; 
		joyStick.SetActive(false); 	
		pauseButton.SetActive (false);
	}

	//except pause button
    public void RemoveEveryUI() {
        ResetUI();

    }

	//including pause button
	public void RemoveAllUI() {
		ResetUI();
		pauseButton.SetActive (false);
	}

    public void EnableEveryUI(){
		
		touchManager.enabled = true;      
		freeMove.enabled = true; 
		waveSelection.SetActive(true);
        pauseButton.SetActive(true);

    }

	public void EnableResumeUI(){

		touchManager.enabled = true;      
		freeMove.enabled = true; 
		pauseButton.SetActive(true);

	}
		
	public void Flip(string direction){

		float waveSelectionXPosition = waveSelection.GetComponent<RectTransform> ().anchoredPosition.x;
		float waveSelectionYPosition = waveSelection.GetComponent<RectTransform> ().anchoredPosition.y;
	
		float anchorMinX, zoneAnchorMinX;

		float scaleWaveSelectorX;

		float joyStickZoneXPosition = joyStickZone.GetComponent<RectTransform> ().anchoredPosition.x;
		float joyStickZoneYPosition = joyStickZone.GetComponent<RectTransform> ().anchoredPosition.y;
		float anchorMaxX, zoneAnchorMaxX;

		bool newDirection = false;


		if (direction == "right") {
			scaleWaveSelectorX = 1;
			anchorMinX = anchorMaxX = 1;
			zoneAnchorMinX = zoneAnchorMaxX = 0;

			if (waveSelectionXPosition > 0) {
				joyStickZoneXPosition = - joyStickZoneXPosition;
				waveSelectionXPosition = - waveSelectionXPosition;
				newDirection = true;
			}

		} else {
			anchorMinX = anchorMaxX = 0;
			zoneAnchorMinX = zoneAnchorMaxX = 1;
			scaleWaveSelectorX = - 1;

			if (waveSelectionXPosition < 0) {
				joyStickZoneXPosition = - joyStickZoneXPosition;
				waveSelectionXPosition = - waveSelectionXPosition;
				newDirection = true;
			}
		}

		if (newDirection) {
			
			waveSelection.GetComponent<RectTransform> ().anchorMin = new Vector2 (anchorMinX, 0);
			waveSelection.GetComponent<RectTransform> ().anchorMax = new Vector2 (anchorMaxX, 0);
			waveSelection.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (waveSelectionXPosition, waveSelectionYPosition);
			waveSelection.GetComponent<RectTransform> ().localScale = new Vector3 (scaleWaveSelectorX, 1, 1);

			foreach (Transform button in waveSelection.transform) {

				button.localScale = new Vector3 (scaleWaveSelectorX, 1, 1);

			}

			joyStickZone.GetComponent<RectTransform> ().anchorMin = new Vector2 (zoneAnchorMaxX, 0);
			joyStickZone.GetComponent<RectTransform> ().anchorMax = new Vector2 (zoneAnchorMaxX, 1);
			joyStickZone.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (joyStickZoneXPosition, joyStickZoneYPosition);

		}

	}

}
