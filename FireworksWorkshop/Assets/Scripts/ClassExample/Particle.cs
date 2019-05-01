using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private Vector3 movementVector;
    private float lifespan;
    private Vector3 downVector = Vector3.down;

    public bool useGravity = false;
    public float useGravityAterInterval = 1f;
    public float gravityStrenght = 4f;
    public float initialLifespan = 1f;
    public float lifespanDeviation = 1f;
    public float speed = 1f;
    public float speedDeviation = 1f;
    public float size = 1f;
    public float sizeDeviation = 1f;
    public ParticleHandler particleDelegate;

    void Start()
    {
        SetInitialValues();
    }

    public void SetInitialValues()
    {
        InitialMovementVector();

        lifespan = initialLifespan + Random.Range(-lifespanDeviation / 2, lifespanDeviation / 2);

        useGravity = false;
        useGravityAterInterval = 1f;
    }

    protected virtual void InitialMovementVector()
    {
        movementVector = Vector3.zero;
        movementVector += Vector3.up * (speed + Random.Range(-speedDeviation / 2, speedDeviation / 2));
        movementVector += Vector3.left * (speed + Random.Range(-speedDeviation / 2, speedDeviation / 2));
        movementVector += Vector3.forward * (speed + Random.Range(-speedDeviation / 2, speedDeviation / 2));

        transform.localScale = Vector3.one * (size + Random.Range(-sizeDeviation / 2, sizeDeviation / 2));
    }

    private void Update()
    {
        if (!useGravity && (useGravityAterInterval -= Time.deltaTime) <= 0)
            useGravity = true;

        if (useGravity)
            movementVector += downVector * gravityStrenght * Time.deltaTime;
        transform.position += movementVector;

        if ((lifespan -= Time.deltaTime) <= 0)
        {
            if (particleDelegate != null)
                particleDelegate.ParticleBecomeInactive(this);
            else
                Destroy(this);
        }
    }
}
