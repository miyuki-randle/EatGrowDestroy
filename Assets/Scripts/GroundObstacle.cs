using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundObstacle : MovingObject, IEventListener<GameEvent>
{
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip impactSFX;
    [SerializeField] private int maxLives = 3;
    [SerializeField] private int curLives = 0;
    public Slider healthBar;
    public bool breakable = false;

    new void Start()
    {
        base.Start();
        curLives = maxLives;
        healthBar.maxValue = maxLives;
        healthBar.value = curLives;
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Platform") if (source.enabled) source.PlayOneShot(impactSFX);
        if (breakable && collision.gameObject.tag == "Laser")
        {
            curLives -= 1;
            healthBar.value = curLives;
            if (curLives == 0)
            {
                Instantiate(explosionParticle, transform.position, transform.rotation);
                GameManager.Instance.score += 200;
                Destroy(gameObject);
            }
        }
    }
}
