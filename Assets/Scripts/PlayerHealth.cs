
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    public AudioClip[] audioClips;
    Rigidbody rigidBody;
    AudioSource playerAudio;
    public ScreenFader screenFader;
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    private Image fillRectImage;
    public Image vampBatImage;
	Color vampBatEndColor;
	public AudioClip deathClip;
    public float flashSpeed = 1f;
    public float colorSpeed = 14f;
    public Color flashColour;
    public bool isDead, vampDamage;
    VampireBat vampireScript;
    public Slingshot slingShot;
    public Text miniScoreText;
    private float textPositionY;
    int upDistance = 15;
    float textSpeed = 30f;
    public bool up;
    public bool down, hitSoundCoroutine ;
	bool colorGreen;
	public Animator batAnim;

    void Awake()
    {
		up = false;
		vampDamage = false;
		vampBatEndColor = new Color (255f, 0f, 0f, 0f);
        fillRectImage = healthSlider.fillRect.GetComponent<Image>();
        rigidBody = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        vampireScript = GetComponent<VampireBat>();
        textPositionY = miniScoreText.transform.localPosition.y;
		colorGreen = false;
		hitSoundCoroutine = false;
		down = false;

        //   Debug.Log(textPositionY);


    }

    void Start()
    {

        startingHealth = GameManager.health;
		#if UNITY_EDITOR
		if(startingHealth == 0)
			startingHealth = 100;
		#endif
        currentHealth = startingHealth;
        healthSlider.maxValue = startingHealth;
        healthSlider.value = healthSlider.maxValue;
        healthSlider.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(startingHealth, 0);



    }


    public void TakeDamage(int amount)
    {


		if (!isDead) {
			if (vampireScript.isVamp) {
				vampDamage = true;
			} else {
				currentHealth -= amount;
				vampDamage = false;
				healthSlider.value -= amount;
			}

			if (playerAudio.isPlaying)
				playerAudio.Stop ();
		
			playerAudio.clip = audioClips [0];
			playerAudio.Play ();

			if (!hitSoundCoroutine)
				StartCoroutine (hitSoundPlay ());

		}
			if (currentHealth <= 0 && !isDead) {
				vampBatImage.enabled = true;
				rigidBody.isKinematic = true;
				slingShot.enabled = false;
				Death ();
			}
			if (!isDead) {
				vampBatImage.enabled = true;
				vampBatImage.color = flashColour;
				StopCoroutine (RedColorFade ());
				StartCoroutine (RedColorFade ());
			}
		
    }


    void Death()
    {
        StopAllCoroutines();
		batAnim.SetBool ("isFlying", false);
		batAnim.SetBool ("isDrinking", false);
		batAnim.SetTrigger("isDead");
        isDead = true;
        vampBatImage.color = flashColour;
        screenFader.EndScene();

        playerAudio.clip = deathClip;
        playerAudio.Play();
    }

    private IEnumerator RedColorFade()
    {
        while (vampBatImage.color.a > 0.04f)
        {
            
            vampBatImage.color = Color.Lerp(vampBatImage.color, vampBatEndColor, flashSpeed * Time.deltaTime);
            yield return null;
        }
        vampBatImage.color = vampBatEndColor;
        vampBatImage.enabled = false;
    }

    private IEnumerator SliderGreenColor()
	{
        while (fillRectImage.color.b > .05f && colorGreen)
        {
			
            fillRectImage.color = Color.Lerp(fillRectImage.color, Color.green, flashSpeed * Time.deltaTime);
            yield return null;
        }
		fillRectImage.color = Color.white;
	
    }
		

    public bool AddHealth(int amount)
    {
        
            currentHealth += amount;
			colorGreen = true;
            AnimateText(amount);
		if (currentHealth > healthSlider.maxValue) {
			healthSlider.maxValue = currentHealth;
			healthSlider.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(currentHealth, 0);
		}
			healthSlider.value = currentHealth;
		
			StopCoroutine(SliderGreenColor());
            StartCoroutine(SliderGreenColor());

            return true;
        

    }
    public bool CheckHealth()
    {
        return currentHealth == startingHealth;
    }
    private void AnimateText(int amount)
    {

        miniScoreText.text = "+" + amount;
        miniScoreText.enabled = true;
        StartCoroutine(TextUp());
    }

    private IEnumerator TextUp()
    {


        float towards = textPositionY + upDistance;
        float limit = towards - 0.1f;
        while (miniScoreText.transform.localPosition.y < limit)
        {
            miniScoreText.transform.localPosition = new Vector3(miniScoreText.transform.localPosition.x,
                Mathf.MoveTowards(miniScoreText.transform.localPosition.y, towards, Time.deltaTime * textSpeed),
                miniScoreText.transform.localPosition.z);
            yield return null;
        }
        yield return new WaitForSeconds(.2f);
        miniScoreText.transform.localPosition = new Vector3(miniScoreText.transform.localPosition.x, textPositionY,
            miniScoreText.transform.localPosition.z);
		colorGreen = false;
        miniScoreText.enabled = false;

    }

    public int damageTaken()
    {

        return startingHealth - currentHealth;
    }
    private IEnumerator hitSoundPlay()
    {
		hitSoundCoroutine = true;
        while (playerAudio.isPlaying)
            yield return null;
        playerAudio.clip = audioClips[1];
        playerAudio.Play();
		hitSoundCoroutine = false;
    }
}
