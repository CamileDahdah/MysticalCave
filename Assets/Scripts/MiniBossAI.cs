using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossAI : MonoBehaviour {

	//enum for miniBoss states
	public enum EnumStates { Idle, Attack1, Attack2, Hurt, Death }
	public EnumStates state;
	private Animator miniBossAnim;
	private GameObject player;
	private Transform target;
	private PlayerHealth playerHealth;
	public float rotationalSpeed = 1f, moveSpeed = 1f;
	Vector3 targetPoint, toInitialPosition;
	Quaternion targetRotation;
	float realDistance;
	public Transform initialTransform;
	public Transform referenceTransform;
	public GameObject electricOrb;
	float distance, timer, timeLimit;
	bool canRotate;
	float limit = 8.5f;
	bool followBat;
	public TrailRenderer trailRenderer1, trailRenderer2;
	public GameObject tipColliderLeft, tipColliderRight;
	int enemyLayer;
	public MiniBossHealth miniBossHealth;
	public GameObject end;
	public static bool canKill;
	Damage damage;
	public AudioClip slashAttackAudio;
	private AudioSource audioSource;

	void Awake(){
		audioSource = GetComponent<AudioSource> ();
		enemyLayer = LayerMask.NameToLayer ("Enemy");
		canKill = false;
	}


	void Start () {
		miniBossAnim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		target = player.transform;
		playerHealth = player.GetComponent<PlayerHealth> ();
		damage = player.GetComponent<Damage> ();
		canRotate = true;
		timer = 0.0001f;
		timeLimit = 4f;
		State = EnumStates.Idle;
		followBat = false;
		tipColliderLeft.SetActive (false);
		tipColliderRight.SetActive (false);
		trailRenderer1.enabled = false;
		trailRenderer2.enabled = false;
	}



	void Update () {
		if (!playerHealth.isDead) {
			distance = Vector2.Distance (new Vector2 (player.transform.position.x, player.transform.position.z),
				new Vector2 (initialTransform.position.x, initialTransform.position.z));
		
			//follow bat if in boundary
			if (State != EnumStates.Attack1 && distance < 23f && State != EnumStates.Hurt) {
				if (State != EnumStates.Attack2) {
					followBat = true;
					State = EnumStates.Attack2;
				}

			} else {
				//don't follow bat if out of boundary
				if (distance > 25.5f) {
					followBat = false;
					GoToInitialPosition ();
				}

				if (State == EnumStates.Attack2 && State != EnumStates.Idle && State != EnumStates.Hurt) {
					State = EnumStates.Idle;
				} else {
					if (State != EnumStates.Attack1) {
						if (timer == 0) {
							timeLimit = Random.Range (3f, 7f);
						}
						timer += Time.deltaTime;
						if (timer > timeLimit && State != EnumStates.Hurt) {
							State = EnumStates.Attack1;
							timer = 0;
						}
					}
				}
			}
		}
	}

	void FixedUpdate(){
		if (canRotate) {
			targetPoint = new Vector3 (target.position.x, referenceTransform.position.y, target.position.z);
			targetRotation = Quaternion.LookRotation (targetPoint - referenceTransform.position, Vector3.up);
			if (Vector2.Distance (new Vector2 (player.transform.position.x, player.transform.position.z),
				   new Vector2 (transform.position.x, transform.position.z)) > 3.2f) {
				transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, Time.deltaTime * rotationalSpeed);
			}
		}
	}

	void StateManager(){

		//reset everything related to states
		ResetState ();

		switch (state) {

		case EnumStates.Idle:
			Idle ();
			break;

		case EnumStates.Attack2:
			Attack2();
			break;
				
		case EnumStates.Attack1:
			Attack1();
			break;

		case EnumStates.Hurt:
			Hurt ();
			break;

		case EnumStates.Death:
			Death ();
			break;

		}
	}

	void ResetState(){
		if (miniBossAnim) {
			miniBossAnim.SetBool ("idleBool", false);
			miniBossAnim.SetBool ("attack1Bool", false);
			miniBossAnim.SetBool ("attack2Bool", false);
			miniBossAnim.SetBool ("hurtBool", false);
			StopCoroutine ("FollowBat_Coroutine");
		}
		if (ElectricOrb.ballInProcess) {
			electricOrb.SetActive (false);
			ElectricOrb.ballInProcess = false;
		}
		canRotate = true;
		if(!(State == EnumStates.Attack1 || State == EnumStates.Attack2))
		StartCoroutine(TrailRendererStatus (false, 0.65f));
	}

	//Attack near
	void Attack2(){
		StartCoroutine("FollowBat_Coroutine");
	}

	//Attack with ball
	void Attack1(){
		StartCoroutine(TrailRendererStatus (true, 0f));
		canRotate = false;
		miniBossAnim.SetBool ("attack1Bool", true);
		electricOrb.SetActive (true);

	}
	void Idle(){
		miniBossAnim.SetBool ("idleBool", true);

	}

	public EnumStates State {

		get { 
			return state; 
		}

		set{
			state= value;
			// !!! Call StateManager() every time I set state variable !!!
			//if(!isDead)
				StateManager ();
		}


	}

	IEnumerator FollowBat_Coroutine(){

		while(followBat){
			realDistance = Vector2.Distance (new Vector2 (player.transform.position.x, player.transform.position.z),
				new Vector2 (transform.position.x, transform.position.z));
			if (realDistance > limit) {
				transform.position += transform.forward * moveSpeed * Time.deltaTime;
				yield return new WaitForFixedUpdate();
			} 
			else if(realDistance < limit - .5f){
				if (distance > 27.5f) {
					followBat = false;
					GoToInitialPosition ();
					break;
				} else {
					transform.position -= transform.forward * moveSpeed * Time.deltaTime * 2;
					yield return new WaitForFixedUpdate();
				}
			}
			else {
				if (miniBossAnim.GetCurrentAnimatorStateInfo (0).IsName ("Attack2")) {
					miniBossAnim.Play ("Attack2", 0, 0f);
				}
				miniBossAnim.SetBool ("attack2Bool", true);
				StartCoroutine(TrailRendererStatus (true, 0f));
				audioSource.clip = slashAttackAudio;
				//dependent on if else if above conditions
				Invoke ("PlaySlashSound", .67f);
				break;
			}

		}

	}

	void PlaySlashSound(){
		if (state == EnumStates.Attack2 && realDistance >= limit - .5f && realDistance <= limit) {
			audioSource.Play ();
		}
	}

	void GoToInitialPosition(){
		if (State != EnumStates.Attack1) {
			toInitialPosition = Vector3.MoveTowards (transform.position, initialTransform.position, Time.deltaTime * moveSpeed);
			transform.position = new Vector3 (toInitialPosition.x, transform.position.y, toInitialPosition.z);
		}
	}

	IEnumerator TrailRendererStatus(bool status, float time){
		yield return new WaitForSeconds(time);
		if ((State == EnumStates.Attack1 || State == EnumStates.Attack2) && !status) {
			
		} else {
			trailRenderer1.enabled = status;
			trailRenderer2.enabled = status;
		}
	}
	public void Attacking2(int enabled){

		if (enabled == 1) {
			//isAttacking2 = true;

		} else{
			//isAttacking2 = false;

		}
	}
	public void HitAttack2(){

		if (canKill) {
			playerHealth.TakeDamage (99999999);
			damage.OverHeadAnim (null);
		}
	}

	public void FinishAttack2(){
		State = EnumStates.Idle;
	}

	void Hurt(){
		miniBossAnim.SetBool ("hurtBool", true);
		if (miniBossAnim.GetCurrentAnimatorStateInfo (0).IsName ("Hurt")) {
			miniBossAnim.Play ("Hurt", 0, 0);
		}

	}

	public void FinishHurt(){
		State = EnumStates.Idle;;
	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.layer == enemyLayer) {
			miniBossHealth.Damage (LevelManager.miniBossDamageHit);
			if (!miniBossHealth.isDead) {
				State = EnumStates.Hurt;
			}
		}
	}

	void Death(){
		miniBossAnim.SetTrigger ("deathTrigger");
		Invoke("EndStar", 2.25f);
		this.enabled = false;

	}
	public void FinishDeath(){
		miniBossHealth.DeathAnimationFinished ();
	}

	void EndStar(){
		end.SetActive (true);
	}
}
