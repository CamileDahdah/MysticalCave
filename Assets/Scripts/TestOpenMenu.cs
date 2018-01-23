using UnityEngine;

public class TestOpenMenu : MonoBehaviour {

    public GameObject menu, text;

	void Start () {
        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("OpenMenu") == 1){
            menu.SetActive(true);
            text.SetActive(false);
        }
    }
	
}
