using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleClear : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!particle.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
