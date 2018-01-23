using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
#if UNITY_EDITOR || UNITY_STANDALONE
    Rigidbody rigidBody;
    public float speed = .4f;
	float rotationSpeed = 4f;
    void Awake()
    {
		
        rigidBody = GetComponent<Rigidbody>();

    }


    void Update()
    {

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = transform.forward * moveVertical;
            movement = movement.normalized;
            rigidBody.velocity = movement * speed;
		    Vector3 newRotation = new Vector3(0, moveHorizontal*rotationSpeed, 0);
            transform.Rotate(newRotation);


    }
#endif
}
