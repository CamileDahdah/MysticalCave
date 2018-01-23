using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follow : MonoBehaviour {
	
	NavMeshAgent agent, targetAgent;
	private Transform target; 
	NavMeshPath path;
	public EnemyStates enemyStates;
	float timer;

	void Start () {
		path = new NavMeshPath ();
		agent = GetComponent<NavMeshAgent> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		targetAgent = GameObject.FindGameObjectWithTag ("Player").GetComponent<NavMeshAgent> ();
		agent.isStopped = false;
		timer = 1;
	}


	void Update () {
//		Debug.Log (agent.velocity.magnitude);
		if (agent.isActiveAndEnabled && enemyStates.State != EnemyStates.EnumStates.Hurt && enemyStates.State != EnemyStates.EnumStates.Attacking) {
			agent.SetDestination (new Vector3 (targetAgent.transform.position.x,
				targetAgent.transform.position.y, targetAgent.transform.position.z));
			if (agent.velocity.magnitude == 0) {
				timer += Time.deltaTime;
				if (timer > .1f) {
					enemyStates.State = EnemyStates.EnumStates.Idle;
				} 
			}else {
				timer = 0;
				enemyStates.State = EnemyStates.EnumStates.Walking;
			}

		} else {
			
			agent.velocity = Vector3.zero;
			if (!agent.isActiveAndEnabled && enemyStates.State != EnemyStates.EnumStates.Hurt && enemyStates.State != EnemyStates.EnumStates.Attacking) {
				enemyStates.State = EnemyStates.EnumStates.Idle;
			}
		}
		if (agent.isActiveAndEnabled) {
			var lookPos = target.position - transform.position;
			lookPos.y = 0;
			var rotation = Quaternion.LookRotation (lookPos);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * 1.4f);

		} else {
		}
	}
}
