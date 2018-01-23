using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class VampireBat : MonoBehaviour
{

    GameObject bat;
    PlayerHealth playerHealth;
    public Slider vampSlider;
    float timer;
    private CameraPosition cameraPosition;
    public GameObject cameraObject;
    private Transform headCameratransform;
    Material normalMaterial;
    public Material vampMaterial;
    public bool isVamp = false;
    public Animator baatAnimator;
    bool isDrinking = false;
    public Light vampLight;
    Rigidbody rigidBody;
    public Image damageImage;
    public float vampBatDuration;
    bool up, check;
    bool isAnimating;
    float value;
    float percentage, alphaPercentage;
    float newTimer, otherTimer, oldValue, oldColor, thePosition;
    public SkinnedMeshRenderer renderer1;
	public WaveSelectionManager waveSelectionManager;
	public Slingshot slingShot;
	float bloodPositionY;
	bool animate;
	public Image radialImage;
	RigidbodyConstraints rbConstraints;

    void Awake(){
		
		animate = false;
        headCameratransform = cameraObject.transform.parent.GetComponent<Transform>();
        cameraPosition = cameraObject.GetComponent<CameraPosition>();
        bat = GameObject.FindGameObjectWithTag("Player");
        normalMaterial = renderer1.material;
       
        PlayerPrefs.SetFloat("oldvalue", vampSlider.value);
        PlayerPrefs.SetFloat("oldcolor", damageImage.color.a);
		radialImage.fillAmount = 0;
        playerHealth = bat.GetComponent<PlayerHealth>();
        rigidBody = bat.GetComponent<Rigidbody>();
		rbConstraints = rigidBody.constraints;
    }
    void OnEnable(){
		
        vampBatDuration = 12f;
        vampSlider.value = PlayerPrefs.GetFloat("oldvalue");
		radialImage.fillAmount = 0;
        damageImage.color = new Color(255f, 0, 0, PlayerPrefs.GetFloat("oldcolor"));
        isAnimating = true;
        percentage = vampSlider.value / vampBatDuration;
        alphaPercentage = damageImage.color.a / vampBatDuration;
        check = true;
		thePosition = bat.transform.position.y;
        timer = 0;
        up = false;
        isDrinking = true;
        newTimer = 0;
        otherTimer = 0;
		slingShot.currentWaves [1] += Random.Range(1, 3); 

		if (!animate) {
			isVamp = true;
			waveSelectionManager.UpdateText ();
			renderer1.material = vampMaterial;
			vampLight.enabled = true;
			vampSlider.gameObject.SetActive (true);
			radialImage.gameObject.SetActive (true);
			damageImage.enabled = true;
		} else {
			baatAnimator.SetTrigger ("isDrinking");
			rigidBody.constraints |= RigidbodyConstraints.FreezePosition;
		}
     
	
    }

  
    void Update(){

        if (isVamp)
            timer += Time.deltaTime;

		if (timer > 12f || playerHealth.vampDamage){
			
            Reset();
        }
        else
        {
			if (animate) {
				if (isDrinking) {

					newTimer += (0.2f * Time.deltaTime);
					Vector3 newPos = transform.position;
					newPos.y = Mathf.Lerp (newPos.y, bloodPositionY, newTimer);
					transform.position = newPos;

				}
				if (transform.position.y <= bloodPositionY * 1.02) {

					isDrinking = false;
					newTimer = 0;
					up = true;
					otherTimer += Time.deltaTime;

				}
				if (up && otherTimer >= .6f) {
					if (transform.position.y >= thePosition * 0.37) {
						rigidBody.constraints = rbConstraints;
					}
					if (check) {
						newTimer += (0.2f * Time.deltaTime);
						Vector3 newPos = transform.position;
						newPos.y = Mathf.Lerp (newPos.y, thePosition, newTimer);
						transform.position = newPos;
					}
					if (transform.position.y >= thePosition * 0.996) {

						if (check) {
                      
							isAnimating = false;
							renderer1.material = vampMaterial;
							isVamp = true;
							vampLight.enabled = true;
							vampSlider.gameObject.SetActive (true);
							radialImage.gameObject.SetActive (true);
							damageImage.enabled = true;

							transform.position = new Vector3 (transform.position.x, thePosition, transform.position.z);
							check = false;
							waveSelectionManager.UpdateText ();
						}
						Fade ();
						VampSlide ();

					}
				}
			} else {

				Fade ();
				VampSlide ();

			}
        }
    }

    void OnDisable(){
		
		waveSelectionManager.UpdateText ();
		isVamp = false;
		animate = false;
	}
	public void SetBloodHeight(float value){
		bloodPositionY = value;
		animate = true;
	}

    void VampSlide(){

        value = Mathf.MoveTowards(vampBatDuration, 0f, Time.deltaTime);
        vampBatDuration = value;
        float finalvalue = value * percentage;
        vampSlider.value = finalvalue;
		radialImage.fillAmount = 1 - finalvalue;
    }

    public void Fade(){
	  
        float alpha = alphaPercentage * value;
        damageImage.color = new Color(255f, 0, 0, alpha);
    }

    void Reset(){
		
        renderer1.material = normalMaterial;
        vampLight.enabled = false;
        damageImage.enabled = false;
        vampSlider.gameObject.SetActive(false);
		radialImage.gameObject.SetActive(false);
        playerHealth.vampDamage = false;
        isVamp = false;
        this.enabled = false;

    }

    private IEnumerator Animating(){
		
        while (isAnimating){

            Vector3 newPos = new Vector3(headCameratransform.position.x, transform.position.y
                + cameraPosition.distanceY
                ,
                headCameratransform.position.z);
            headCameratransform.position = Vector3.Lerp(headCameratransform.position,
                newPos, cameraPosition.smooth * Time.deltaTime);
            yield return null;
        }
    }

}