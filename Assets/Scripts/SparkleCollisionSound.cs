using UnityEngine;
using System.Collections;

public class SparkleCollisionSound : MonoBehaviour
{
   
    AudioSource audioSource;
 
    void Start(){
        audioSource = GetComponent<AudioSource>();
    }
  
  
    void OnParticleCollision(GameObject other){
		if (other.tag == "Wall" && Time.timeScale == 1){
			if (!audioSource.isPlaying) {
				audioSource.Play ();
			}
        }
    }

}
