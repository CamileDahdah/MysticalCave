using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    static AudioSource collectibleAudioSource;

    void Awake()
    {

        collectibleAudioSource = GetComponent<AudioSource>();
    }


    public static void playCollectibleAudio(AudioClip audioClip, float volume = 1)
    {
        collectibleAudioSource.PlayOneShot(audioClip, volume);
    }

}
