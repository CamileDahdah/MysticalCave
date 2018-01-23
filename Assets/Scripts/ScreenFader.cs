
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{

    Image blackImage;
    public Reset reset;
    public float fadeSpeed;          // Speed that the screen fades to and from black.
    GameObject bat;
    private bool sceneStarting = true;
    public GameObject endPanel;
    public SceneManagerCharbz sceneManagerCharbz;// Whether the scene is still fading in or not .
    bool check;

    void Awake()
    {
        bat = GameObject.FindWithTag("Player");
		blackImage = GetComponent<Image>();

        if (sceneStarting)
        {
            StartScene();
        }
        check = true;
    }


    void StartScene() //invoked when the level starts
    {
		blackImage.enabled = true;
        StartCoroutine(FadeToClear());


    }


    public void EndScene() //invoked when the level ends
    {

		blackImage.enabled = true;        
        StopCoroutine(FadeToClear());
        check = false;
        StartCoroutine(FadeToBlack());

    }

  
    IEnumerator FadeToClear()  //fading functions used whenever the user dies or wins
    {
		while (blackImage.color.a > 0.01f && check)
        {
			blackImage.color = Color.Lerp(blackImage.color, Color.clear, fadeSpeed * Time.deltaTime);
            yield return null;
        }

		blackImage.color = Color.clear;
		blackImage.enabled = false;
        sceneStarting = false;


    }

    IEnumerator FadeToBlack()
    {
		blackImage.color = Color.clear;
		while (blackImage.color.a < 0.995f)
        {

			blackImage.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(blackImage.color.a, 255f, fadeSpeed * Time.deltaTime));
            yield return null;
        }
		yield return new WaitForSeconds (2f);
        sceneManagerCharbz.ReloadScene();

    }
    public IEnumerator FinishToBlack()
    {
        reset.RemoveEveryUI();
        bat.GetComponent<Rigidbody>().isKinematic = true;
		blackImage.enabled = true;

		while (blackImage.color.a < 0.75f)
        {

			blackImage.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(blackImage.color.a, 255f, fadeSpeed * Time.deltaTime));
            yield return null;
        }

        endPanel.SetActive(true);
		while (blackImage.color.a < 0.995f)
        {

			blackImage.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(blackImage.color.a, 255f, fadeSpeed * Time.deltaTime));
            yield return null;
        }
    }
}
