using UnityEngine;

public class Collectibles : MonoBehaviour
{
    private GameObject bat;
    private PlayerHealth playerHealth;
    bool firstTime;
	float volume = 1f;
	public AudioClip audioClip;
	VampireBat vampireBat;

    void Awake()
    {

        bat = GameObject.FindWithTag("Player");
        playerHealth = bat.GetComponent<PlayerHealth>();
		firstTime = true;
		vampireBat = bat.GetComponent<VampireBat> ();

    }

    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.tag == "Player")
        {

			if (tag == "BloodJar") {
				vampireBat.enabled = false;
				vampireBat.enabled = true;
				transform.parent.gameObject.SetActive (false);
		
			}
        }
    }

        void OnTriggerStay(Collider other)
    {
		if (tag == "BloodJar") {


		} else {
			if (other.gameObject.tag == "Player") {

				if (firstTime && playerHealth.AddHealth (25)) {
					firstTime = false;
					SoundManager.playCollectibleAudio (audioClip, volume);
					Destroy (gameObject.transform.parent.gameObject);
				}
			}
		}

    }

    void OnTriggerExit(Collider other)
    {
		if (tag == "BloodJar") {


		} else {

		}
    }
  

}
