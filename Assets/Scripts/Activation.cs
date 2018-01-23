using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Activation : MonoBehaviour
{
    public GameObject dynamicLight;
    public Text secretRoomText;
    private bool isSecretRoom;
    float duration = 4f;
    public static int secretRoomCounter;
	public static int currentRoomCounter, allRooms;
    AudioSource audioSource;
    BoxCollider boxCollider;

    void Awake()
    {
        if (secretRoomText == null)
            isSecretRoom = false;
        else
            isSecretRoom = true;
        dynamicLight.SetActive(false);
		currentRoomCounter = 0;
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private IEnumerator TurnOffText(float duration)
    {
        yield return new WaitForSeconds(duration);
        secretRoomText.enabled = false;



    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            if (isSecretRoom)
            {
                secretRoomCounter++;
                secretRoomText.enabled = true;
                StartCoroutine(TurnOffText(duration));
            }

            dynamicLight.SetActive(true);
            audioSource.Play();
            boxCollider.enabled = false;

        }
    }
}