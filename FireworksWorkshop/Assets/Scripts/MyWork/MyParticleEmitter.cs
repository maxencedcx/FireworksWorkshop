using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyVector3Event : UnityEvent<Vector3>
{ }


public class MyParticleEmitter : MonoBehaviour
{
    [SerializeField] private bool isSubEmitter = false;
    [SerializeField] private int burstAmount = 0;
    [SerializeField] private float burstInterval = 0;
    [SerializeField] private int emissionAmount = 0;
    [SerializeField] private float emissionInterval = 0;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private float lifespan = 1;
    [SerializeField] private float lifespanDeviation = 0;
    [SerializeField] private float size = 1;
    [SerializeField] private float sizeDeviation = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private float speedDeviation = 1;
    [SerializeField] private Vector3 direction = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 directionDeviation = new Vector3(0, 0, 0);
    [SerializeField] private MyVector3Event onDeath;
    [SerializeField] private MyVector3Event onBirth;
    [SerializeField] private int MaximumParticles = 100;

    private Stack<MyParticle> inactiveParticles = new Stack<MyParticle>();
    private List<MyParticle> activeParticles = new List<MyParticle>();
    private float emissionTimer = 0f;
    private float burstTimer = 0f;

    private void Start()
    {
        if (!isSubEmitter && burstAmount != 0)
        {
            for (int i = 0; i < burstAmount; ++i)
                CreateParticle();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < activeParticles.Count; ++i)
            if (!activeParticles.ToArray()[i].Update())
                DisableParticle(i);

        float particlesToSpawn = 0;

        if (emissionInterval != 0)
        {
            emissionTimer += Time.deltaTime;
            double amountToAdd = System.Math.Truncate(emissionAmount / (1 / emissionTimer));
            if (amountToAdd > 0)
            {
                particlesToSpawn += System.Convert.ToInt32(amountToAdd);
                emissionTimer = 0;
            }
        }

        burstTimer += Time.deltaTime;
        if (burstInterval != 0 && burstTimer >= burstInterval)
        {
            particlesToSpawn += burstAmount;
            burstTimer = 0;
        }

        for (int i = 0; i < particlesToSpawn; ++i)
            CreateParticle();
    }

    private void CreateParticle(Vector3? pos = null)
    {
        if (activeParticles.Count < MaximumParticles)
        {
            MyParticle p = null;
            float l = lifespan + Random.Range(-lifespanDeviation / 2, lifespanDeviation / 2);
            float si = size + Random.Range(-sizeDeviation / 2, sizeDeviation / 2);
            float sp = speed + Random.Range(-speedDeviation / 2, speedDeviation / 2);
            Vector3 d = direction + new Vector3(Random.Range(-directionDeviation.x / 2, directionDeviation.x / 2),
                                                Random.Range(-directionDeviation.y / 2, directionDeviation.y / 2),
                                                Random.Range(-directionDeviation.z / 2, directionDeviation.z / 2));

            if (inactiveParticles.Count > 0)
            {
                p = inactiveParticles.Pop();
                p.Reset(l, si, sp, d);
                p.gameObject.transform.position = (isSubEmitter && pos != null) ? (pos.Value) : (transform.position);
                p.gameObject.SetActive(true);
            }
            else
            {
                GameObject g = GameObject.Instantiate(particlePrefab, (isSubEmitter && pos != null) ? (pos.Value) : (transform.position), Quaternion.identity, transform);
                p = new MyParticle(g, l, si, sp, d);
            }

            onBirth.Invoke(p.gameObject.transform.position);
            activeParticles.Add(p);
        }
    }

    private void DisableParticle(int i)
    {
        onDeath.Invoke(activeParticles[i].gameObject.transform.position);
        activeParticles[i].gameObject.SetActive(false);
        inactiveParticles.Push(activeParticles[i]);
        activeParticles.RemoveAt(i);
    }

    public void SubEmitterTrigger(Vector3 pos)
    {
        for (int i = 0; i < burstAmount; ++i)
            CreateParticle(pos);
    }
}
