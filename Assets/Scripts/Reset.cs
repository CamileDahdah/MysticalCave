using UnityEngine;




public class Reset :MonoBehaviour{
  

    public GameObject baat;
    public GameObject joyStick, waveSelection;
    public TouchManager touchManager;
    public FreeMove freeMove;
    public SpriteRenderer finishFlag,batEyes;
    public GameObject pauseButton, mapCanvas;
    public static Reset instance = null;
	public GameObject HUDCanvas;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

	//remove every UI excluding pause button and map button for now
    public void ResetUI() {

        touchManager.enabled = false;      
		freeMove.enabled = false; 
		joyStick.SetActive(false); 
		waveSelection.SetActive(false);
		HUDCanvas.SetActive (false);

    }

    public void EnableMapUIs() {
        finishFlag.enabled = !finishFlag.enabled;
        batEyes.enabled = !batEyes.enabled;
        baat.SetActive(!baat.activeSelf); 
    }
	//except pause button
    public void RemoveEveryUI() {
        ResetUI();
       // pauseButton.SetActive(false);
        //mapCanvas.SetActive(false);
    }

    public void EnableEveryUI()
    {
		touchManager.enabled = true;      
		freeMove.enabled = true; 
		joyStick.SetActive(true); 
		waveSelection.SetActive(true);
        pauseButton.SetActive(true);
//        mapCanvas.SetActive(true);
		HUDCanvas.SetActive (true);
    }


}
