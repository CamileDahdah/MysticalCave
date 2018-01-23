using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour {

	void OnTriggerEnter(Collider other){

		if (other.tag == "Player" && !other.isTrigger) {
			MiniBossAI.canKill = true;
			EnemyStates.attackRange = true;

		}
	}

	void OnTriggerStay(Collider other){

		if (other.tag == "Player" && !other.isTrigger) {
			MiniBossAI.canKill = true;
			EnemyStates.attackRange = true;
		}
	}

	void OnTriggerExit(Collider other){

		if (other.tag == "Player" && !other.isTrigger) {
			MiniBossAI.canKill = false;
			EnemyStates.attackRange = false;
		}
	}
}
