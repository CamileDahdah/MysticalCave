using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePortal : MonoBehaviour {

	const int size = 8;
	List<ParticleSystem> listParticle = new List<ParticleSystem> (size);
	List<float> minSize = new List<float>(size);
	List<float> maxSize = new List<float>(size);
	float minSizeC, maxSizeC;
	float speed = .5f;


	void Start () {
		GetComponent<AudioSource> ().PlayDelayed (1f);
		foreach (ParticleSystem p in transform.GetComponentsInChildren<ParticleSystem>()) {
			listParticle.Add (p);
			p.Stop ();
			p.Clear ();
			var mainP = p.main;
			var startSize = mainP.startSize;

			minSize.Add (startSize.constantMin);
			maxSize.Add (startSize.constantMax);

			mainP.startSize = startSize;
			startSize.constantMax = 0;
			startSize.constantMin = 0;
			mainP.startSize = startSize;
			p.Play ();

		}

		StartCoroutine ("AnimatePortal_Coroutine");
	}
	
	IEnumerator AnimatePortal_Coroutine(){

		var mainP = listParticle [listParticle.Count - 1].main;
	
		while (	mainP.startSize.constantMax < maxSize[listParticle.Count - 1]) {

			for (int i = 0; i < listParticle.Count; i++) {
				var mainPC = listParticle [i].main;
				var startSize = mainPC.startSize;
				minSizeC = Mathf.MoveTowards (startSize.constantMin, minSize[i], Time.deltaTime * speed * minSize[i]);
				maxSizeC = Mathf.MoveTowards (startSize.constantMax, maxSize[i], Time.deltaTime * speed * maxSize[i]);
				startSize.constantMin = minSizeC;
				startSize.constantMax = maxSizeC;
				mainPC.startSize = startSize;

			}

			yield return null;
		}

	}
		
}
