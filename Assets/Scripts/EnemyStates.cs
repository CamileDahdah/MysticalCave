using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStates : MonoBehaviour {

	//enum for enemy states
	public enum EnumStates { Walking, Attacking, Hurt, Dead, Idle }
	public EnumStates state;
	public Animator enemyAnim;
	public Rigidbody rigidBody;
	public NavMeshAgent agent;
	private Transform target;
	float AttackDistance = 2.4f;
	public Transform referenceTransform;
	bool canAttack ;
	public Follow follow;
	bool isDead;
	public SphereCollider attackRangeCollider;
	public static bool attackRange;
	private PlayerHealth playerHealth;
	private GameObject player;
	public AudioClip[] audioClips;
	AudioSource audioSource;
	public Transform hipTransform;
	public EnemyHealth enemyHealth;
	int enemyLayer;


	void Awake(){
		enemyLayer = LayerMask.NameToLayer ("Enemy");
		player = GameObject.FindGameObjectWithTag ("Player");
		audioSource = GetComponent<AudioSource> ();
		playerHealth = player.GetComponent<PlayerHealth> ();
		target = player.transform;
	}

	void OnEnable () {
		canAttack = true;
		isDead = false;
		State = EnumStates.Idle;
		enemyAnim.SetBool ("Die", false);
		rigidBody.isKinematic = false;
		follow.enabled = true;
		this.enabled = true;
		enemyAnim.enabled = true;
		agent.enabled = true;
		attackRange = false;
	}
		
	void Update(){
		//timer += Time.deltaTime;
		if (canAttack && attackRange == true) {
			if (!playerHealth.isDead && State != EnumStates.Hurt) {
				State = EnumStates.Attacking;
			}
		}
	}

	void StateManager(){
		//reset everything related to states
		ResetState ();
		switch (state) {

		case EnumStates.Walking:
			Walk ();
			break;

		case EnumStates.Idle:
			Idle ();
			break;

		case EnumStates.Attacking:
			Attack ();
			break;

		case EnumStates.Hurt:
			Hurt ();
			break;

		case EnumStates.Dead:
			Dead ();
			break;
		}
	}

	void ResetState(){
		enemyAnim.SetBool ("Hurt", false);
		enemyAnim.SetBool ("Die", false);
		enemyAnim.SetBool ("Walk", false);
		enemyAnim.SetBool ("Attack", false);
		enemyAnim.SetBool ("Idle", false);
	}

	void Walk(){
		enemyAnim.SetBool ("Walk", true);
		if (audioSource.clip != null || audioSource.clip != audioClips [0]) {
			audioSource.clip = audioClips [0];
		}
		if (!audioSource.isPlaying) {
			audioSource.Play ();
		}
	}

	void Attack(){
		agent.velocity = Vector3.zero;
		canAttack = false;
		enemyAnim.SetBool ("Attack", true);
		audioSource.clip = null;
		StartCoroutine ("Attack_Coroutine");
	}

	void Hurt(){
		agent.velocity = Vector3.zero;
		audioSource.clip = audioClips [1];
		audioSource.Play ();
		enemyAnim.SetBool ("Hurt", true);
	}

	void Idle(){
		audioSource.clip = null;
		enemyAnim.SetBool ("Idle", true);
	}

	void Dead(){
		agent.velocity = Vector3.zero;
		audioSource.clip = audioClips [1];
		audioSource.Play ();
		Invoke ("DeathSound", .69f);
		audioSource.loop = false;
		isDead = true;
		agent.enabled = false;
		//agent.Stop();
		enemyAnim.SetBool ("Die", true);
		rigidBody.isKinematic = true;
		follow.enabled = false;
		attackRange = false;
		enemyHealth.enabled = false;
		attackRangeCollider.enabled = false;
		//	LevelManager.DecreaseTotalEnemyCount ();
		//	LevelManager.instance.overallScore += 10;
		this.enabled = false;

	}

	//getter and setter
	public EnumStates State {
		get { 
			return state; 
		}

		set{
			state= value;
			// !!! Call StateManager() every time I set state variable !!!
			if(!isDead)
			StateManager ();
		}
	}

	//when hurt animation has finished trigger this event
	public void HurtAnimationFinished(){
		State = EnumStates.Idle;
		canAttack = true;
	}

	public void AttackAnimationFinished(){
		if (Vector3.Distance (transform.position, target.position) < AttackDistance) {
			//some follow logic here
			if (Physics.Raycast (referenceTransform.position, referenceTransform.forward, 5f)) {
				if(!playerHealth.isDead && State != EnumStates.Hurt)
					State = EnumStates.Attacking;
				//Some attack logic here
			} else {
				State = EnumStates.Idle;
				canAttack = true;
			}
		} else {
			State = EnumStates.Idle;
			canAttack = true;
		}

	}

	void OnTriggerEnter(Collider collider){

		if (collider.gameObject.layer == enemyLayer) {
			enemyHealth.Damage (50);
			if (!enemyHealth.isDead) {
				State = EnumStates.Hurt;
			}
		}
	}
		
	IEnumerator Attack_Coroutine(){
		yield return new WaitForSeconds (.25f);
		if (attackRange == true) {
			playerHealth.TakeDamage (LevelManager.gollumDamageHit);
		}
	}

	public void SetHip(){
		hipTransform.localPosition = new Vector3(0, 1.156f, -0.03f);
		hipTransform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
	}

	void DeathSound(){
		audioSource.clip = audioClips[2];
		audioSource.Play ();
	}

}
