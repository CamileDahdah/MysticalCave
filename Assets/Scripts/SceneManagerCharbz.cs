using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCharbz : MonoBehaviour {

	public PauseManager pauseManager;
   
    void Awake() 
	{
		
        Time.timeScale = 1;           
            
	}

	public void ReloadScene()
	{
		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}
		GameManager.Instance.ReloadLevel ();
		
	}

	public void LoadMenu()
	{
       
        SceneManager.LoadScene("Camille intro");
	}
   
	public void QuitGame()
	{
		Application.Quit();
	}

}
