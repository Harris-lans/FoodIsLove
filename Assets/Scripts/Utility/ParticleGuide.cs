using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGuide : MonoBehaviour
{
    [SerializeField]
    int SpeedMult = 10;
    [SerializeField]
    float ParticleDelay = 2.0f;
    public ParticleSystem Particlefollow;
    public Transform Target;


    private void Awake()
    {
        Particlefollow = GetComponent<ParticleSystem>();
        Particlefollow.Stop();
    }

    public void InitiateParticleFlow(Transform target)
    {
        Particlefollow.Play();
        StartCoroutine(TimeDelay());
    }

    private IEnumerator TimeDelay()
    {
        yield return new WaitForSeconds(ParticleDelay);

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[Particlefollow.particleCount];

		while (true)
		{
			Debug.Log("Working");
			particles = new ParticleSystem.Particle[Particlefollow.particleCount];
			Particlefollow.GetParticles(particles);

			// Modifying the particles

			for (int i = 0; i < particles.GetLength(0); i++)
			{
				float ForceToAdd = (particles[i].startLifetime - particles[i].remainingLifetime) * (SpeedMult * Vector3.Distance(Target.position, particles[i].position));
				particles[i].velocity = (Target.position - particles[i].position).normalized * ForceToAdd;
			}

			Particlefollow.SetParticles(particles, particles.Length);

			yield return null;
		}
    }
}
