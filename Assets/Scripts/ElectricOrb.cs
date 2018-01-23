using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricOrb : MonoBehaviour {

	Animator tombStoneAnim;
	public Transform ElectricOrbLoc;
	const int size = 6;
	List<ParticleSystem> listParticle = new List<ParticleSystem> (size);
	List<float> minSize = new List<float>(size);
	List<float> maxSize = new List<float>(size);
	float minSizeC, maxSizeC, ballLightIntensityMax;
	float animationSpeed = .66f;
	Rigidbody rigidBody;
	public float ballSpeed = 5f;
	public MiniBossAI miniBossAI;
	public static bool ballInProcess;
	public Light ballLight;
	public GameObject ballImpact;
	Vector3 collisionPosition;
	Transform batTransform;
	bool canFollow;
	float closerSpeed = 10f;

	void Awake(){
		batTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		rigidBody = GetComponent<Rigidbody> ();
		foreach (ParticleSystem p in transform.GetComponentsInChildren<ParticleSystem>()) {
			listParticle.Add (p);
			p.Stop ();
			p.Clear ();
			var mainP = p.main;
			var startSize = mainP.startSize;

			minSize.Add (startSize.constantMin);
			maxSize.Add (startSize.constantMax);
			ballInProcess = false;

		}
		ballLightIntensityMax = ballLight.intensity;
	
	}


	void OnEnable () {
		ballImpact.SetActive (false);
		rigidBody.velocity = Vector3.zero;
		transform.position = ElectricOrbLoc.position;
		transform.rotation = ElectricOrbLoc.rotation;
		foreach (ParticleSystem p in transform.GetComponentsInChildren<ParticleSystem>()) {
	
			var mainP = p.main;
			var startSize = mainP.startSize;
			mainP.startSize = startSize;
			startSize.constantMax = 0;
			startSize.constantMin = 0;
			mainP.startSize = startSize;
			p.Play ();
			ballInProcess = true;
		}
		ballLight.intensity = 0;
		canFollow = false;
		StartCoroutine ("AnimateElectricOrb_Coroutine");
	}

	void Update(){
		if (canFollow) {
			transform.position = Vector3.MoveTowards (transform.position, batTransform.position, Time.deltaTime * closerSpeed);
		}
	}

	IEnumerator AnimateElectricOrb_Coroutine(){

		var mainP = listParticle [listParticle.Count - 1].main;

		while (	mainP.startSize.constantMax < maxSize[listParticle.Count - 1]) {

			for (int i = 0; i < listParticle.Count; i++) {
				var mainPC = listParticle [i].main;
				var startSize = mainPC.startSize;
				minSizeC = Mathf.MoveTowards (startSize.constantMin, minSize[i], Time.deltaTime * animationSpeed * minSize[i]);
				maxSizeC = Mathf.MoveTowards (startSize.constantMax, maxSize[i], Time.deltaTime * animationSpeed * maxSize[i]);
				startSize.constantMin = minSizeC;
				startSize.constantMax = maxSizeC;
				mainPC.startSize = startSize;

			}
			ballLight.intensity = Mathf.MoveTowards (ballLight.intensity, ballLightIntensityMax, Time.deltaTime * animationSpeed * ballLightIntensityMax);
			yield return null;
		}
		ballInProcess = false;
		yield return new WaitForSeconds(.32f);
		rigidBody.velocity = transform.forward * ballSpeed;
		yield return new WaitForSeconds(.4f);

		canFollow = true;
		yield return new WaitForSeconds(.65f - .4f);
		miniBossAI.State = MiniBossAI.EnumStates.Idle;
	}



	void OnCollisionEnter(Collision c){
		
			collisionPosition = c.contacts[0].point;
			if (c.gameObject.tag == "TombStone") {

				tombStoneAnim = c.gameObject.GetComponent<Animator> ();
		
				if (!tombStoneAnim.GetBool ("First")) {
					tombStoneAnim.SetBool ("First", true);

				} else if (!tombStoneAnim.GetBool ("Second")) {
					tombStoneAnim.SetBool ("Second", true);
				
				} else if (!tombStoneAnim.GetBool ("Third")) {
					tombStoneAnim.SetBool ("Third", true);
					tombStoneAnim.GetComponent<BoxCollider> ().enabled = false;


				} else {


				}
		
				Recharge ();

			} else if (c.gameObject.tag == "Player") {
				
				Recharge ();

			} else if (c.gameObject.tag == "Wall") {

				Recharge ();
			}


	}

	void OnDisable(){
		if(miniBossAI.State == MiniBossAI.EnumStates.Attack1)
		if(miniBossAI)
			miniBossAI.State = MiniBossAI.EnumStates.Idle;
	}

	void Recharge(){
		

		gameObject.SetActive (false);
		ballImpact.SetActive (true);
		ballImpact.transform.position = collisionPosition;
		ballImpact.transform.rotation = transform.rotation;
	
	}


}
