using UnityEngine;
using System.Collections;

public class GemCollectible : MonoBehaviour
{
	public GameObject LightGem;
    public Transform gemDoorLoc;
    public Transform gem;
	public static bool isPicked;
    float volume = 1f;
    public AudioClip audioClip;

	void Start()
	{
		isPicked = false;

	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
			LightGem.SetActive (true);
            SoundManager.playCollectibleAudio(audioClip, volume);
            gem.position = gemDoorLoc.position;
            Transform parent = transform.parent;
            gem.parent = parent.parent;
            gem.gameObject.SetActive(false);
            isPicked = true;
            Destroy(parent.gameObject);

        }

    }

}
