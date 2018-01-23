using UnityEngine;

public class FreeMove : MonoBehaviour 
{
    public float speed;
    public Slingshot slingScript;
  	Vector2 mousePos ;
 	Vector3 screenPos ;
    public VirtualJoyStick joyStick;
    Rigidbody playerRigidbody;
	public float rotationSpeed = 1.5f;
	float moveHorizontal, moveVertical;
	Vector3 movement, newRotation;

    void Awake(){
        playerRigidbody = GetComponent<Rigidbody>();
    }


#if UNITY_EDITOR || UNITY_STANDALONE
    void Update() {

        if (UnityEditor.EditorApplication.isRemoteConnected){
            MoveMobile();

        }else{

            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
            movement = transform.forward * moveVertical;
            movement = movement.normalized;
            playerRigidbody.velocity = movement * speed;
			//Vector3 newRotation = new Vector3(0, Mathf.Exp(moveHorizontal * rotationSpeed) - Mathf.Exp(0), 0);
			Vector3 newRotation =  new Vector3(0, Mathf.Pow(1.8f, Mathf.Abs(moveHorizontal) * rotationSpeed) - 1, 0);
			if (moveHorizontal < 0)
				newRotation = -newRotation;
            transform.Rotate(newRotation);    

        }     
    }

#else
    void Update(){
      MoveMobile();
    }
#endif

    void OnDisable() {
        playerRigidbody.velocity = Vector3.zero;
		playerRigidbody.angularVelocity = Vector3.zero;
		moveVertical = 0;
		moveHorizontal = 0;
    }

   void  MoveMobile() {
        moveHorizontal = joyStick.Horizontal();
        moveVertical = joyStick.Vertical();
        Vector3 movement = transform.forward * moveVertical;
        playerRigidbody.velocity = movement * speed;
		Vector3 newRotation = new Vector3(0, moveHorizontal * rotationSpeed, 0);
        transform.Rotate(newRotation);

    }
}
