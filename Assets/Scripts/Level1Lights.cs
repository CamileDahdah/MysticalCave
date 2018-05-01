using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Lights : MonoBehaviour {

	public List<GameObject> lights = new List<GameObject>();
	private float maxIntensity;
	private float limit = 0f;
	float duration = 14f;
	float secondDuration = 1.5f;
	private float maxParticleSize;
	int particleCounter = 4;
	float intensity, size ;


	void Awake () {

		maxIntensity = lights[0].GetComponent<Light>().intensity;
		//fifth light is a particle system
		var main = lights [particleCounter].transform.parent.GetComponentInChildren<ParticleSystem> ().main;
		maxParticleSize = main.startSize.constant;

	}

	void OnEnable()
	{
		StartCoroutine (changeLightIntensity());
	}


	IEnumerator changeLightIntensity () 

	{
		while (true) {
			
			while (lights [0].GetComponent<Light> ().intensity > limit) {

				intensity = Mathf.MoveTowards (lights [0].GetComponent<Light> ().intensity, 0f, Time.deltaTime);
				size =  Mathf.MoveTowards (lights [particleCounter].transform.parent.GetComponentInChildren<ParticleSystem> ().main.startSize.constant, 0f, Time.deltaTime);

				for (int i = 0; i < lights.Capacity; i++) {
			

					lights [i].GetComponent<Light> ().intensity = intensity;


				}
				for (int i = particleCounter; i < lights.Capacity; i++) {

					var main = lights [i].transform.parent.GetComponentInChildren<ParticleSystem> ().main;
					main.startSize = size;



				}
				yield return null;
			}
				

			yield return new WaitForSeconds (duration);

			while (lights [0].GetComponent<Light> ().intensity < (maxIntensity - limit)) {

				intensity = Mathf.MoveTowards (lights [0].GetComponent<Light> ().intensity, maxIntensity, Time.deltaTime);
				size =  Mathf.MoveTowards (lights [particleCounter].transform.parent.GetComponentInChildren<ParticleSystem> ().main.startSize.constant, 
					maxParticleSize, Time.deltaTime);

				for (int i = 0; i < lights.Capacity; i++) {

						
					lights [i].GetComponent<Light> ().intensity = intensity;



				}
				for (int i = particleCounter; i < lights.Capacity; i++) {


					var main = lights [i].transform.parent.GetComponentInChildren<ParticleSystem> ().main;
					main.startSize = size;



				}
				yield return null;
			}

			yield return new WaitForSeconds (secondDuration);
		}

	}
}
