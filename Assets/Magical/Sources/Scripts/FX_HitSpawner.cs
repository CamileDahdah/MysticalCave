using UnityEngine;
using System.Collections;

namespace MagicalFX
{
	public class FX_HitSpawner : MonoBehaviour
	{


		public GameObject FXSpawn;
		public bool desactiveOnHit = false;
		public bool FixRotation = false;
		public float LifeTimeAfterHit = 1;
		public float LifeTime = 0;
		int enemyLayer;

		void Awake ()
		{
			enemyLayer = LayerMask.NameToLayer ("Enemy");
		}
	
		void Spawn ()
		{
			if (FXSpawn != null) {
				Quaternion rotate = this.transform.rotation;
				if (!FixRotation)
					rotate = FXSpawn.transform.rotation;
					FXSpawn.transform.position = this.transform.position;
					FXSpawn.transform.rotation = rotate;
					FXSpawn.SetActive (true);
				if (LifeTime > 0)
					StartCoroutine (Desactivate(LifeTime));
			}
			if (desactiveOnHit) {

				this.gameObject.SetActive(false);
				//if (this.gameObject.GetComponent<Collider>())
				//	this.gameObject.GetComponent<Collider>().enabled = false;

			}
		}
	
		void OnTriggerEnter (Collider other)
		{
			if(other.tag == "Wall" || other.tag == "TombStone" || other.gameObject.layer == enemyLayer || 
				other.gameObject.tag == "ElectricOrb")
			Spawn ();
		}
	
		void OnCollisionEnter (Collision other)
		{
			if(other.gameObject.tag == "Wall" || other.gameObject.tag == "TombStone" || 
				other.gameObject.layer == enemyLayer || other.gameObject.tag == "ElectricOrb")
			Spawn ();
		}

		IEnumerator Desactivate(float lifeTime){
			yield return new WaitForSeconds (lifeTime);
			FXSpawn.SetActive (false);

		}
	}
}