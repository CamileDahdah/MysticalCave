using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Slingshot : MonoBehaviour{
	
    public AudioClip[] audioclips;
    float dist;
    bool isNotBack = true;
    float timer, audioTimer;
    RaycastHit2D hit;
    private ParticleSystem[] particleSystems;
    public TouchManager touchManager;
    [HideInInspector]
    public int[] maxWaves;
    public int[] currentWaves;
    ParticleSystem.Particle[] particles;
    public GameObject bat;
	public Transform waveEmission;
    private GameObject[] waveParticle;
    public bool  moveRotBool;
    public float timeBetweenBullets = 0.15f, range = 100f;
    public AudioSource waveAudio;
    public AudioSource[] waveBackAudio;
    Rigidbody batRb;
    int touchOfScreenId;
    public Slider waveSlider;
    AudioSource[] wavesAudio;
    public int numWaves;
    public WaveSelectionManager waveSelectionManager;
    public Light lightParticle;
	public GameObject particleSystemsObject, waveParticle1;
    float[] particleSpeed;
    int particleSystemSize;
	public GameObject darkMissileWave;
	public Animator batAnimator;
	public VampireBat vampireBat;
	public Damage damage;

    void Start(){
		
        particleSystemSize = particleSystemsObject.transform.childCount;

        wavesAudio = waveAudio.GetComponents<AudioSource>();
        float volf = PlayerPrefs.GetFloat("wavevolume", 0.6f);
        waveSlider.value = volf;
        wavesAudio[0].volume = waveSlider.value;
        wavesAudio[1].volume = waveSlider.value;
        currentWaves = new int[4];
        currentWaves[0] = GameManager.normalWave;
        currentWaves[1] = GameManager.attackWave;
        currentWaves[2] = GameManager.bounceWave;
        currentWaves[3] = GameManager.lightWave;
		#if UNITY_EDITOR
			if(GameManager.normalWave == 0){
				for(int i= 0; i < 4; i++)
					currentWaves[i] = 50;
			}

		#endif
        particleSystems = new ParticleSystem[particleSystemSize];
        waveParticle = new GameObject[particleSystemSize];
        particleSpeed = new float[particleSystemSize];

		waveParticle[0] = waveParticle1;
		particleSystems[0] = waveParticle[0].GetComponent<ParticleSystem>();
		particleSpeed[0] = particleSystems[0].startSpeed;
        for (int i = 1; i < particleSystemSize; i++){
			
            waveParticle[i] = particleSystemsObject.transform.GetChild(i).gameObject;
            particleSystems[i] = waveParticle[i].GetComponent<ParticleSystem>();
			particleSpeed[i] = particleSystems[i].startSpeed;

        }

		particles = new ParticleSystem.Particle[particleSystems[4].main.maxParticles];
        audioTimer = 20f;
        batRb = bat.GetComponent<Rigidbody>();
        moveRotBool = false;
    }


    void FixedUpdate(){
        timer += Time.deltaTime;
        audioTimer += Time.deltaTime;
	}

	public void SendWaveButton() {
		//Debug.Log (numWaves);
		if (timer >= timeBetweenBullets && Time.timeScale != 0
			&& numWaves >= 0 && audioTimer > 1.5f && damage.isActive){
				WaitForConstraints();

		}
	}

    void WaitForConstraints(){
        StopAllCoroutines();
		//sparkle particle
        if (numWaves == 0){
            int index = 0;
            setParticleDefaultSpeed(index);
            wavesAudio[index].clip = audioclips[index];
            ParticleSystemEmission(particleSystems[index],0);
            StartCoroutine(ParticleSpeed(particleSystems[index]));
            PlayWave();

        }

        //bounce particle
        else if (numWaves == 2){
            int index = 5;
            int angle = 15;
            int spaceAngle = angle;
            wavesAudio[0].clip = audioclips[1];
            for (int i = index - 3; i < index; i++){
                setParticleDefaultSpeed(i);
                ParticleSystemEmission(particleSystems[i], angle);
                StartCoroutine(ParticleSpeed(particleSystems[i]));
                angle -= spaceAngle;
            }
            PlayWave();
        }

        //light particle
		else if (numWaves == 3){
            int index = 1;
            wavesAudio[0].clip = audioclips[2];
            setParticleDefaultSpeed(index);
            ParticleSystemEmission(particleSystems[index], 0);
            StartCoroutine(ParticleSpeed(particleSystems[index]));
            StartCoroutine(LightFollow(index));
            PlayWave();

        }
		//Vampire dark missile particle
		else if (numWaves == 1 && vampireBat.isVamp){
            //wavesAudio[index].clip = audioclips[index];
			batAnimator.SetBool ("isAttacking", true);
			audioTimer = 0f;
			Invoke ("AttackAnim", .45f);
		
        }
		if ((vampireBat.isVamp && numWaves == 1) || numWaves != 1) {
			currentWaves [numWaves]--;
			waveSelectionManager.UpdateText ();
		}
    }

    private void setParticleDefaultSpeed(int i){
        particleSystems[i].startSpeed = particleSpeed[i];
    }

    void ParticleSystemEmission(ParticleSystem particleSystem, float angle){

        particleSystem.transform.rotation = Quaternion.Euler(0, waveEmission.rotation.eulerAngles.y + angle, 0);
        particleSystem.transform.position = new Vector3(waveEmission.position.x,waveEmission.position.y,
			waveEmission.position.z);
        particleSystem.Stop();
        particleSystem.Clear();
        particleSystem.Play();

    }

        IEnumerator ParticleSpeed(ParticleSystem particleSystem){
	        float startSpeed = particleSystem.startSpeed;
	        int count = particleSystem.GetParticles(particles);
	      
	        while (count == 0){
	            float speed = -transform.InverseTransformDirection(batRb.velocity).x;
	            particleSystem.startSpeed = startSpeed + speed;
	            count = particleSystem.GetParticles(particles);
	            yield return null;
	        }
	        
	        float timer = 0;
	        
	        while (count > 0 && particleSystem.isPlaying && isNotBack){
			
	            float speed = -transform.InverseTransformDirection(batRb.velocity).x;
	            particleSystem.startSpeed = startSpeed + speed;
	            particleSystem.transform.position = new Vector3(waveEmission.transform.position.x,
	            waveEmission.position.y, waveEmission.position.z);
	            count = particleSystem.GetParticles(particles);
	            
	            timer += Time.deltaTime;
	            yield return null;
	        }
	        particleSystem.startSpeed = startSpeed;

	    }

    void PlayWave(){
		audioTimer = 0f;
        StopAllAudioBack();
        waveAudio.Stop();
        waveAudio.Play();
    }

 /*
            Vector3 incomingVec = shootHit[i].point - rayEmission.transform.position;
            Vector3 reflectVec = Vector3.Reflect(incomingVec, shootHit[i].normal);
            // Debug.DrawLine(rayemission.transform.position, shootHit[i].point, Color.red);
            //  Debug.DrawRay(shootHit[i].point, reflectVec, Color.green);
            referenceForward[i] = shootHit[i].normal;
            referenceRight[i] = Vector3.Cross(Vector3.up, -incomingVec);
            newDirection[i] = reflectVec;

           */
   
  
    private IEnumerator LightFollow(int index){
		
        int count = particleSystems[index].GetParticles(particles);
        while (count == 0){
            count = particleSystems[index].GetParticles(particles);
            yield return null;

        }
        lightParticle.enabled = true;

        while (count > 0){
            lightParticle.transform.position = particles[0].position;
            count = particleSystems[index].GetParticles(particles);
            yield return null;
        }

        lightParticle.enabled = false;
    }

    public void SetWavesAmount(List<int> waveamount){
        for (int i = 0; i < currentWaves.Length; i++){
            currentWaves[i] = waveamount[i];
        }
    }

    public void WaveSoundVolume(){
        wavesAudio[0].volume = waveSlider.value;
        wavesAudio[1].volume = waveSlider.value;
        PlayerPrefs.SetFloat("wavevolume", waveSlider.value);
    }

    void StopAllAudioBack(){
        foreach (AudioSource audioSource in waveBackAudio){
            audioSource.Stop();
        }
    }

    void StopAllParticleSystems(){
        foreach (ParticleSystem particleSystem in particleSystems){
           particleSystem.Clear();

        }
			
    }
	void AttackAnim(){
		if(damage.isActive){
			darkMissileWave.SetActive (false);
			darkMissileWave.transform.rotation = Quaternion.Euler(0, waveEmission.rotation.eulerAngles.y, 0);
			darkMissileWave.transform.position = new Vector3 (waveEmission.position.x,
				waveEmission.position.y, waveEmission.position.z);
			darkMissileWave.SetActive (true);
			PlayWave();
			audioTimer -= .5f;
		}
		else
			audioTimer = 0f;
	}
}