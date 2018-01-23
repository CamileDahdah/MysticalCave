using UnityEngine;
using System.Collections;

public class BounceHitSound : MonoBehaviour {

    AudioSource audioSource;

   // public ParticleSystem particleSystem;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

      //  particleSystem = gameObject.GetComponent<ParticleSystem>();

    }

    void OnParticleCollision(GameObject other)
    {
		if (other.tag == "Wall" && Time.timeScale == 1)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
            /*
            particleSystem.Emit(particleSystem.maxParticles);
            ParticleSystem.Particle[] particleArray = new ParticleSystem.Particle[particleSystem.maxParticles];
            int particles = particleSystem.GetParticles(particleArray);
            for (int i = 0; i < particles; i++)
            {
                Vector3 vel = particleArray[i].velocity;
                vel.y = 0;
                particleArray[i].velocity = vel;
            }
            particleSystem.SetParticles(particleArray, particles);
            */

        }
    }
}
