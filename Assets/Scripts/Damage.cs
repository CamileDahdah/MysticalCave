
using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    
    PlayerHealth playerHealth;
	GameObject bat, bloodCopy;
    public GameObject overHead;
    public GameObject blood, bloodFx;
    public float force;
    Rigidbody batRb;
    public Animator baatAnimator;
    float timer = 1.5f;
    public bool isActive ;
    Scale scaleScript;
	float ratio, angle;
	ParticleSystem[] bloodFxChildren;
	ContactPoint c ;
	Vector3 normalVector;
	public VampireBat vampireBat;

    void Start()
    {
        bat = GameObject.FindGameObjectWithTag("Player");
        playerHealth = bat.GetComponent<PlayerHealth>();
        batRb = bat.GetComponent<Rigidbody>();
		ratio = Scale.instance._Scale/ 0.275f;
		bloodFxChildren = bloodFx.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < bloodFxChildren.Length; i++)
		{
			var main = bloodFxChildren[i].main;
			main.startSizeMultiplier *= ratio;
			main.startSpeedMultiplier *= ratio;

		}
		isActive = true;
    }

    void OnCollisionEnter(Collision collision)
    {
		if (!playerHealth.isDead) {
			if (collision.gameObject.tag == "Wall" && timer >= 1.5f) {
				//No damage if licking blood from puddle and hits ground
				if (!(vampireBat.enabled && !vampireBat.isVamp)) {
					timer = 0;
          
					playerHealth.TakeDamage (25);

					OverHeadAnim (collision);
					BloodParticle ();
					StartCoroutine (InstantiateBloodFx (c));
				}

			} else if (collision.gameObject.tag == "ElectricOrb") {
			
				playerHealth.TakeDamage (25);
				OverHeadAnim (collision);
				StartCoroutine (InstantiateBloodFx (c));

			} 
		}
    }
	/*void OnTriggerEnter(Collider collider){

		if (collider.gameObject.tag == "Death" && !playerHealth.isDead) {

			playerHealth.TakeDamage (9999999);
			//StartCoroutine (InstantiateBloodFx (c));
		}
	}*/

	public void OverHeadAnim(Collision collision){
		if (!playerHealth.isDead) {
			overHead.SetActive (true);
			isActive = false;
			if (!baatAnimator.GetCurrentAnimatorStateInfo (2).IsName ("isHitanim")) {

				if (!baatAnimator.GetAnimatorTransitionInfo (2).IsUserName ("HitTransition"))
					baatAnimator.SetTrigger ("isHit");  
			
			} else {
				baatAnimator.Play ("isHitanim", 2, 0f);
				//baatAnimator.ResetTrigger("isHit");
			}
			if (collision != null) {
				c = collision.contacts [0];
				normalVector = c.normal;
				batRb.AddForce (normalVector.normalized * force);
			}

			StartCoroutine (WaitForConstraints ());
		}
	}

	private void BloodParticle(){

		angle = Vector3.Angle(new Vector3(1, 0, 0), normalVector);
		bloodCopy = (GameObject)Instantiate(blood, new Vector3(c.point.x, c.point.y, c.point.z), Quaternion.Euler(90, angle - 90, 0));
		bloodCopy.transform.localScale *= Scale.instance.scale;


	}
    void Update()
    {

        timer += Time.deltaTime;
    }




    IEnumerator WaitForConstraints()
    {
      
        yield return new WaitForSeconds(.06f);
		//FREEZE
        batRb.constraints |= RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
        yield return new WaitForSeconds(2.2f);
        isActive = true;
		//UNFREEZE
        batRb.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
        yield return new WaitForSeconds(.3f);
        if(isActive==true)
        overHead.SetActive(false);

    }

	IEnumerator InstantiateBloodFx(ContactPoint c)
	{
		
		yield return new WaitForSeconds (.1f);
		bloodFx.SetActive (true);
		for (int i = 0; i < bloodFxChildren.Length; i++){
			bloodFxChildren [i].transform.position = new Vector3 (c.point.x, c.point.y, c.point.z);
			bloodFxChildren [i].Play ();
		}
		yield return new WaitForSeconds (1f);
		bloodFx.SetActive (false);
	}

}
