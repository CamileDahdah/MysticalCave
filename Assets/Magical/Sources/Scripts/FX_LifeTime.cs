using UnityEngine;
using System.Collections;

namespace MagicalFX
{
	public class FX_LifeTime : MonoBehaviour
	{

		public float LifeTime = 3;
		public GameObject SpawnAfterDead;
		private float timeTemp;
	
		void OnEnable ()
		{

				timeTemp = Time.time;

		}

		void Update ()
		{
			
			if (Time.time > timeTemp + LifeTime) {
				this.gameObject.SetActive(false);
					if (SpawnAfterDead != null) {
						GameObject.Instantiate (SpawnAfterDead, this.transform.position, SpawnAfterDead.transform.rotation);
				}
			}
		}
	}
}