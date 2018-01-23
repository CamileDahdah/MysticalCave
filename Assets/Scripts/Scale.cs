using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Scale : MonoBehaviour {

	public List<Transform> gameObjectsList;
	//public List<ParticleSystem> sceneParticles;
	public float scaleGameObjects;
	[HideInInspector]
    public float scale = 1;
    public Light[] lights;
    public Transform cameraTransform,correctCameraTransform;
    public GameObject batGameObject;
    public ParticleSystem[] particleSystems;
    int sizeLight,sizeParticleSystem;
    public CameraPosition cameraPosition;
    public Camera camera;
    public Transform headCamera;
   // public Transform spawnPoint;
    Damage damageScript;

    public static Scale instance;


    void Awake () {

		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}
		
		if (transform.position.x != 0) {
			scale = transform.position.x;
		} else {
			scale = 1;
		}

        damageScript = batGameObject.GetComponent<Damage>();
        damageScript.force *= 2*scale;
        sizeLight = lights.Length;
        camera.farClipPlane *= scale;
        sizeParticleSystem = particleSystems.Length;
        batGameObject.GetComponent<FreeMove>().speed  *= scale;
        for (int i = 0; i < sizeLight; i++)
        {
            lights[i].range *= scale;
        }
        lights[sizeLight-1].range *= 2;

 
        for (int i = 0; i < sizeParticleSystem; i++)
        {
			var main = particleSystems[i].main;
			main.startSizeMultiplier *= 2*scale;
            if(i!=sizeParticleSystem-1)
            main.startSpeedMultiplier *= 2*scale;
         
        }
  
        ParticleSystem.ShapeModule shapeModule = particleSystems[0].shape;
        //ParticleSystem.ShapeModule shapeModulesSparkle = particleSystems[sizeParticleSystem - 1].shape;
        shapeModule.radius *= 2 * scale;
      //  shapeModulesSparkle.radius *= 2 * scale;

    }
    void Start() {

        cameraPosition.distanceY *= scale;
	
    }

	#if UNITY_EDITOR
	[MenuItem("File/SCALE")]
	static void ScaleLightsAndPX()
	{
		foreach(Transform roomScale in instance.gameObjectsList)
		{
			foreach (Light light in roomScale.GetComponentsInChildren<Light>()) {
					Debug.Log ("it worked");
					light.range *= instance.scaleGameObjects;
					light.areaSize *= instance.scaleGameObjects;

			}
			foreach (ParticleSystem particle in roomScale.GetComponentsInChildren<ParticleSystem>()) {
		
					var main = particle.main;
					main.startSizeMultiplier *= instance.scaleGameObjects;
					main.startSpeedMultiplier *= instance.scaleGameObjects;
					var shape = particle.shape;
		

						shape.radius *= instance.scaleGameObjects;
						Debug.Log ("it worked1");
					
			}
			foreach (Camera camera in roomScale.GetComponentsInChildren<Camera>()) {

				camera.farClipPlane *= instance.scaleGameObjects;

			}
		}
	}
	#endif

	public float _Scale{
		
		get{ return scale;}
		set{
			float ratio;
			ratio = value / scale;
			scale = value;

			batGameObject.transform.localScale = new Vector3(scale,scale,scale);
			for (int i = 0; i < sizeLight; i++)
			{
				lights[i].range *= ratio;
			}
			lights[sizeLight-1].range *= ratio;
		}
	}

	public float GetInitalScale(){

			return transform.position.x;
	
	}
}
