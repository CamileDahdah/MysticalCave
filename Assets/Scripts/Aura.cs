using UnityEngine;
using System.Collections;
using System;

public class Aura :MonoBehaviour
{

    public Animator doorAnim;
    public GameObject gem;
    public GameObject cameraDoor;
    static bool onlyOnce;
    static int counter;

	void Start(){
		counter = 0;
		onlyOnce = true;

	}

    public void OnTriggerEnter(Collider collision){
        counter++;
        if (collision.tag == "Player" && onlyOnce && counter == 2){
			counter = 0;
            if (GemCollectible.isPicked){

                onlyOnce = false;
                CameraSwitch.instance.SwitchCamera(cameraDoor);
                Reset.instance.RemoveEveryUI();
                gem.SetActive(true);
                StartCoroutine(OpenDoor());          


            }
            else{
                PopupTextManager.instance.SetText("You need a Gem to open the door", 3f, true);
            }

        }
    }

    private IEnumerator OpenDoor(){
		
        yield return new WaitForSeconds(1);
        doorAnim.enabled = true;

        yield return new WaitForSeconds(.9f);
        gem.GetComponent<Rigidbody>().isKinematic = false;
		gem.GetComponent<Animator> ().enabled = false;
        while (doorAnim.GetCurrentAnimatorStateInfo(0).IsName("dooranim")
            && doorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)           
        yield return null;
        yield return new WaitForSeconds(.8f);
        //switchCAM
        Reset.instance.EnableEveryUI();
        CameraSwitch.instance.SwitchCamera(cameraDoor);
        this.gameObject.SetActive(false);

    }
}
