using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    public Camera mainCamera;
    public static CameraSwitch instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

    }

    public void SwitchCamera(GameObject secondCamera)
    {

        if (secondCamera.activeSelf){
            secondCamera.SetActive(false);
            mainCamera.transform.parent.position = secondCamera.transform.position;
            float x = mainCamera.transform.rotation.eulerAngles.x;
            mainCamera.transform.parent.rotation = Quaternion.Euler(secondCamera.transform.rotation.eulerAngles.x-x,
				secondCamera.transform.rotation.eulerAngles.y, secondCamera.transform.rotation.eulerAngles.z);
            mainCamera.enabled = true;
        }
		else if(mainCamera.enabled){
            secondCamera.SetActive(true);
            mainCamera.enabled = false;

        }


    }

	public void SwitchToMainCamera(GameObject secondCamera){	
		
			secondCamera.SetActive(false);
			mainCamera.transform.parent.position = secondCamera.transform.position;
			float x = mainCamera.transform.rotation.eulerAngles.x;
			mainCamera.transform.parent.rotation = Quaternion.Euler(secondCamera.transform.rotation.eulerAngles.x-x,
			secondCamera.transform.rotation.eulerAngles.y, secondCamera.transform.rotation.eulerAngles.z);
			mainCamera.enabled = true;

	}
}
