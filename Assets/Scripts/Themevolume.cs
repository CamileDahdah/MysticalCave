using UnityEngine;
using UnityEngine.UI;

public class Themevolume : MonoBehaviour
{
    public Slider themeSlider;
    AudioSource themeSong;

    void Start()
    {
        themeSong = GetComponent<AudioSource>();

        float volf = PlayerPrefs.GetFloat("themevolume", 0.7f);

        themeSlider.value = volf;

        themeSong.volume = themeSlider.value;


    }

    public void ThemeSoundVolume()
    {


        themeSong.volume = themeSlider.value;

        PlayerPrefs.SetFloat("themevolume", themeSlider.value);

    }
}
