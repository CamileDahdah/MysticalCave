using UnityEngine;

public class Arrow : MonoBehaviour
{

    public Transform target;


    void Update()
    {
        transform.LookAt(target);
    }
}
