using UnityEngine;
using UnityEngine.UI;


public class VirtualJoyStick : MonoBehaviour
{
    public Image bgImg, joyStickImg;
    private Vector3 inputVector;
    public TouchManager touchManager;
    GameObject bat;
    Touch[] touch;
    RectTransform rectTransform;
	Vector2 pos;

    void Awake(){
        rectTransform = joyStickImg.GetComponent<RectTransform>();
    }

    void OnEnable(){
        Nulling();

    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            touch = touchManager.touch;
            if (touchManager.id == touch[i].fingerId){
                switch (touch[i].phase){
				// Record initial touch position.
					case TouchPhase.Began:
						Move(touch[i]);
	                    break;

                    case TouchPhase.Moved:
                        Move(touch[i]);
                        break;

                    case TouchPhase.Ended:
                        break;
                }
            }
        }
    }

    public float Horizontal(){
        if (inputVector.x != 0)
            return inputVector.x;
        else return Input.GetAxis("Horizontal");
    }

    public float Vertical(){

        if (inputVector.z != 0)
            return inputVector.z;
        else return Input.GetAxis("Vertical");
    }

    void Move(Touch touch){

        pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform
                                                                    , touch.position
                                                                    , null
																	, out pos);

        pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x) * 2;
        pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y) * 2;

		inputVector = new Vector3(pos.x, 0,  pos.y);
        inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

        // Move Joystick IMG
        rectTransform.anchoredPosition =
         new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 2),
                     inputVector.z * (bgImg.rectTransform.sizeDelta.y / 2));

    }

    public void Nulling(){
        inputVector = Vector3.zero;
        rectTransform.anchoredPosition = Vector3.zero;
    }

	void OnDisable(){
		Nulling ();

	}
}
