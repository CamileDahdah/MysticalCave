using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenResolution : MonoBehaviour {
    public Text resolutionText;

	void Start () {

	}
	

	void Update () {
        resolutionText.text = ""+ Screen.currentResolution;
    }
    public void SetResolution540p()
    {
        Screen.SetResolution(960, 540, true);

    }
    public void SetResolution720p()
    {
        Screen.SetResolution(1280, 720, true);

    }
    public void SetResolution1080p()
    {
        Screen.SetResolution(1920, 1080, true);

    }
    public void SetResolution1440p()
    {
        Screen.SetResolution(2560, 1440, true);

    }

}
