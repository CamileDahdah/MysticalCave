//Written By Camille Dahdah
//Edited By Charbel Daoud 02/01/16

using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Light checkpointLight;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            checkpointLight.enabled = true;
    }
}
