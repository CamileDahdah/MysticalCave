//Written By Camille Dahdah
//Edited By Charbel Daoud 02/01/16

using System;
using System.Collections;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{

    public GameObject bat;
    public float smooth = 1.5f;
    [HideInInspector]
    public float distanceY;
    public float smoothRotation = 3;
    public Transform cameraLoc, cameraForward;
    float a, b, c;
    private float distance;
    public Transform batNeck;
    float timerIn ;
    bool isIn = false;
    float sphereRadius = .25f;
	public static CameraPosition instance;
	float rawDistance;
	float waitDuration = .75f;
	Quaternion lookDirection ;

    void Awake()
    {
		if (instance == null)
			instance = this;
		else
			Destroy(this);
		
		distance = 0;
        distanceY = 2;
        transform.position = bat.transform.position;


    }

    void Start()
    {
		timerIn = 0f;
        sphereRadius *= Scale.instance.scale;
		rawDistance = Vector3.Distance (cameraLoc.position , cameraForward.position);

    }

    void FixedUpdate()
	{

		if (ViewingPosCheck (cameraLoc.position,distance)) {
			isIn = true;
			ZoomIn ();

		} 
		else {
			if (isIn){
				timerIn = 0f;
				isIn = false;
			}
			timerIn += Time.fixedDeltaTime;

			if (timerIn > waitDuration) 
			{
				a = cameraLoc.transform.position.x;
				b = cameraLoc.transform.position.z;
				c = cameraLoc.transform.position.y;
				Vector3 newPos = new Vector3 (a, c, b);
				transform.position = Vector3.Lerp (transform.position, newPos, smooth * Time.deltaTime);
				lookDirection = Quaternion.LookRotation (cameraLoc.transform.forward);

			} 
			else
			{
				ZoomIn ();
			}

		} 
	
		transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, smoothRotation * Time.deltaTime);
    }


	private void ZoomIn()
	{

		LerpCameraForward ();
		Vector3 newPos = new Vector3 (a, c, b);
		lookDirection = Quaternion.LookRotation (cameraForward.transform.forward);

		float realDistance = Vector3.Distance (transform.position , cameraForward.position);
		if (((rawDistance - realDistance) / rawDistance) > .5f)
		{
			float myDistance = Vector3.Distance(batNeck.position,cameraForward.transform.position);
			if (ViewingPosCheck (cameraForward.transform.position, myDistance)) 
			{
				lookDirection = transform.rotation;
				newPos = transform.position;
			}

		}

		transform.position = Vector3.Lerp (transform.position, newPos, smooth * Time.deltaTime);
	

	}

    private void LerpCameraForward()
    {
        a = cameraForward.transform.position.x;
        b = cameraForward.transform.position.z;
        c = cameraForward.transform.position.y;

    }


	bool ViewingPosCheck(Vector3 checkPos,float distance)
    {
        RaycastHit[] hit;
        // If a raycast from the check position to the player hits something...
        Vector3 heading = checkPos - batNeck.position;
      //  var direction = heading / distance;
	
		Debug.DrawRay(batNeck.position, heading, Color.blue);
		hit = Physics.SphereCastAll(batNeck.position, sphereRadius, heading, distance);
        for(int i=0;i<hit.Length;i++)
        {


            // ... if it is not the player...
            if (hit[i].collider.tag == "Wall")
            {
                // This position isn't appropriate.
                return true;
            }
        }
        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        return false;
    }

	public void CalculateDistance()
	{
		distance = Vector3.Distance(batNeck.position,cameraLoc.transform.position);

	}
}

