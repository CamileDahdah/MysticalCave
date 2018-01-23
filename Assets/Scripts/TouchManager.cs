using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour {
	
    public Touch[] touch;
    public int id = -1;
	PointerEventData eventDataCurrentPosition;
	List<RaycastResult> results;
    public VirtualJoyStick virtualJoystickScript;
	public GameObject joyStick;
	public static TouchManager instance;

    void OnEnable(){
		joyStick.SetActive (false);
    }

    void Start () {
		if (instance == null)
			instance = this;
		else
			Destroy (this);
		
        id = -19;
		eventDataCurrentPosition = new PointerEventData(null);
		results = new List<RaycastResult>();

    }
		
	void Update () {

        // Track a single touch as a direction control.
        if (Input.touchCount > 0 && Time.timeScale == 1){

            touch = new Touch[Input.touchCount];

            for (int i = 0; i < Input.touchCount; i++){
				
                touch[i] = Input.GetTouch(i);

                // Handle finger movements based on touch phase.
                switch (touch[i].phase){
                    // Record initial touch position.
					case TouchPhase.Began:
					
						eventDataCurrentPosition.position = touch[i].position;
						results.Clear ();
						EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
						if (results.Count > 0 && results[0].gameObject.name == "JoyStickZone") {
							id = touch [i].fingerId;
							//New joystick positon
							joyStick.SetActive (true);
							joyStick.transform.position = touch[i].position;
						}
	                        break;			

	                // Report that a direction has been chosen when the finger is lifted.
					case TouchPhase.Ended:

						if (id == touch[i].fingerId){
							joyStick.SetActive (false);
	                        id = -1;
	                    }         
	                        break;
                }
            }
        }
    }

	void OnDisable(){
		joyStick.SetActive (false);
	}
}
