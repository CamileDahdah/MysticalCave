using UnityEngine;

public class SaveMenu : MonoBehaviour {
    static SaveMenu instance;

    void Awake()
    {
        if (instance == null)
        {
            PlayerPrefs.SetInt("OpenMenu", 0);
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
	}
	
    public static void SetOpenMenu()
    {
        PlayerPrefs.SetInt("OpenMenu", 1);
    }
    
 
}
