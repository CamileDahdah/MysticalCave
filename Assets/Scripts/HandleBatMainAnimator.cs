using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBatMainAnimator : MonoBehaviour {

	Animator batAnimator;

	void Awake(){

		batAnimator = GetComponent<Animator> ();

	}

	public void EndAttackAnim(){

		batAnimator.SetBool ("isAttacking", false);
		//batAnimator.SetBool ("isAttacking", false);
	}

}
