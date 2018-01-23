using UnityEngine;

public class BloodFloor : MonoBehaviour
{



    public void OnTriggerStay(Collider other)
    {
		if (other.gameObject.tag == "Player")
        {
			GameObject.FindGameObjectWithTag("Player").GetComponent<VampireBat>().SetBloodHeight (transform.position.y);
            other.gameObject.GetComponent<VampireBat>().enabled = true;

        }
    }
}
