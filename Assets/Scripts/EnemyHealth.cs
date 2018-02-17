using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public int currenthealth;
	public int maxHealth;
	public EnemyStates enemyState;
	bool neverHurt;
	public BoxCollider boxCollider;
	public SphereCollider sphereCollider;
	public bool isDead;
	private Animator anim;


	void Awake(){
		anim = GetComponent<Animator> ();
	}


	void OnEnable () {
		maxHealth = 100;
		currenthealth = maxHealth;
		neverHurt = true;
		isDead = false;
		//boxCollider.enabled = true;
		if (sphereCollider != null) {
			sphereCollider.enabled = true;
		}

	}
	
	public void Damage(int damage){
		
		currenthealth -= damage;

		if (neverHurt && currenthealth <= maxHealth / 2) {
			enemyState.State = EnemyStates.EnumStates.Hurt;
			neverHurt = false;
			if (sphereCollider != null) {
				sphereCollider.enabled = false;
			}

		}
		if (currenthealth <= 0) {

			Dead ();
		}

	}

	public void Dead(){
		isDead = true;
		enemyState.State = EnemyStates.EnumStates.Dead;

	}


	public void DeathAnimationFinished(){
		boxCollider.enabled = false;
		if (sphereCollider != null) {
			sphereCollider.enabled = false;
		}
		anim.enabled = false;

	}

	public void SetHealth(int amount){
		currenthealth = amount;
		maxHealth = amount;
	}
}
