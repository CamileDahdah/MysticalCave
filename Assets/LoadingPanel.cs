using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class LoadingPanel : MonoBehaviour {

    public Slider slider;
    
	void OnEnable () {
        slider.value = 0;
        StartCoroutine("UpdateBar");
	}

    private IEnumerator UpdateBar()
    {
        while(GameManager.Instance.async == null){
            yield return null;
        }

        while (GameManager.Instance.async != null && !GameManager.Instance.async.isDone)
        {
            slider.value = GameManager.Instance.async.progress;
            yield return null;
        }

        Time.timeScale = 0;

        while (DynamicGI.isConverged == false){
            yield return null;
        }

        gameObject.SetActive(false);
        Time.timeScale = 1;

    }

}
