using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionClear : MonoBehaviour
{
    private ParticleSystem particleSmoke;

    private void Awake()
    {
        particleSmoke = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!particleSmoke.IsAlive())
        {
            GameManager.Instance.score += 200;
            Destroy(gameObject);
        }
    }
}
