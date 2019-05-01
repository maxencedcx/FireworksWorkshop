using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyParticle
{
    public GameObject gameObject;
    public float lifespan;
    public float size;
    public float speed;
    public Vector3 direction;
    
    public MyParticle(GameObject g, float l, float si, float sp, Vector3 d)
    {
        gameObject = g;
        lifespan = l;
        size = si;
        speed = sp;
        direction = d;
        gameObject.transform.localScale = Vector3.one * size;
    }

    public void Reset(float l, float si, float sp, Vector3 d)
    {
        lifespan = l;
        size = si;
        speed = sp;
        direction = d;
        gameObject.transform.localScale = Vector3.one * size;
    }

    public bool Update()
    {
        if ((lifespan -= Time.deltaTime) <= 0)
            return false;
        else
            gameObject.transform.position += direction;
        return true;
    }
}
