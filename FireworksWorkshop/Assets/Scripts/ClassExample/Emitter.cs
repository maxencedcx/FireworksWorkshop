using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ParticleHandler
{
    void ParticleBecomeInactive(Particle particle);
}

public class Emitter : MonoBehaviour, ParticleHandler
{
    public int emissionRate = 60;
    public int burstEmissionRate = 500;
    public Particle particlePrefab;

    private Stack<Particle> inactiveParticles = new Stack<Particle>();
    private float emissionInterval = 0f;
    private float emissionTimer = 0f;

    private void Start()
    {
        emissionInterval = 1f / emissionRate;

        if (burstEmissionRate > 0)
            for (int i = 0; i < burstEmissionRate; i++)
                CreateParticle();
    }

    private void FixedUpdate()
    {
        emissionTimer += Time.deltaTime;

        while (emissionTimer >= emissionInterval)
        {
            emissionTimer -= emissionInterval;
            CreateParticle();
        }
    }

    private void CreateParticle()
    {
        if (inactiveParticles.Count > 0)
        {
            Particle particle = inactiveParticles.Pop();
            particle.gameObject.SetActive(true);
            particle.transform.position = transform.position;
            particle.SetInitialValues();
        }
        else
        {
            Particle particle = Instantiate(particlePrefab.gameObject, transform.position, Quaternion.identity, transform).GetComponent<Particle>();
            particle.particleDelegate = this;
        }
    }

    public void ParticleBecomeInactive(Particle particle)
    {
        particle.gameObject.SetActive(false);
        inactiveParticles.Push(particle);
    }
}
