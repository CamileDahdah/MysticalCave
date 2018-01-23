using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniBossHealth : MonoBehaviour {

	public int health;
	public int maxHealth;
	public MiniBossAI enemyState;
	public BoxCollider boxCollider;
	public bool isDead;
	private Animator anim;

	public Slider miniBossHealthSlider;               // The slider to represent how much health the tank currently has.
	public Image sliderFillImage;                     // The image component of the slider.
	public Color fullHealthColor = Color.green;       // The color the health bar will be when on full health.
	public Color zeroHealthColor = Color.red;         // The color the health bar will be when on no health.
	public GameObject miniBossCanvas;

	void Awake(){
		anim = GetComponent<Animator> ();
		miniBossCanvas.SetActive (false);
	}


	void Start () {

		isDead = false;
		health = maxHealth;
		miniBossHealthSlider.maxValue = maxHealth;
		SetHealthUI ();
		Invoke ("ActivateUI", 5.7f);
	}

	public void Damage(int damage){

		health -= damage;

		// Change the UI elements appropriately.
		SetHealthUI ();

		if (health <= 0) {

			Dead ();
		}

	}

	public void Dead(){
		isDead = true;
		miniBossCanvas.SetActive (false);
		enemyState.State = MiniBossAI.EnumStates.Death;
		boxCollider.enabled = false;

	}


	public void DeathAnimationFinished(){
	//	boxCollider.enabled = false;
		anim.enabled = false;

	}

	public void SetHealth(int amount){
		health = amount;
		maxHealth = amount;
	}

	private void SetHealthUI ()
	{
		// Set the slider's value appropriately.
		miniBossHealthSlider.value = health;

		// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
		sliderFillImage.color = Color.Lerp (zeroHealthColor, fullHealthColor, ((float)health / maxHealth));

	}
	void ActivateUI(){
		miniBossCanvas.SetActive (true);
	}
}
