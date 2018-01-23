using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PopupTextManager : MonoBehaviour {

    public static PopupTextManager instance;
    private Text textPopup;
    private Animator animatorPopup;
    private float timer = 0;


	void Start () {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        textPopup = GetComponent<Text>();
        animatorPopup = GetComponent<Animator>();
    }

	public void SetText(string newText,float time,bool animated,Color? colorText = null)
    {
		
        StopAllCoroutines();
        textPopup.text = newText;
		textPopup.color = colorText ?? Color.white;
        textPopup.enabled = true;
        if (animated)
        {
            animatorPopup.enabled = true;
            animatorPopup.SetBool("isAnimated", true);

        }
        StartCoroutine(durationText(time,animated));

    }

    private IEnumerator durationText(float time, bool animated)
    {
        while (timer <= time)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        textPopup.enabled = false;
        timer = 0f;
        if (animated)
        {
            animatorPopup.enabled = false;
            animatorPopup.SetBool("isAnimated", false);
        }
    }

}
