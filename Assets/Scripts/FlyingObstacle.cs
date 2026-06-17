using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingObstacle : MovingObject
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip impactSFX;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private int maxLives = 3;
    [SerializeField] private int curLives = 0;
    [SerializeField] private Slider healthBar;

    new void Start()
    {
        base.Start();
        curLives = maxLives;
        healthBar.maxValue = maxLives;
        healthBar.value = curLives;
        source = GetComponent<AudioSource>();
        InvokeRepeating(nameof(SpawnBullet), 1f, fireRate);
    }

    private void SpawnBullet()
    {
        Bullet newObj = Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.localRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Laser")
        {
            if (source.enabled) source.PlayOneShot(impactSFX);
            curLives -= 1;
            healthBar.value = curLives;
            if (collision.gameObject.tag == "Player") SizeHUD.Instance.ShrinkPlayer();
        }
        if (curLives == 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            GameManager.Instance.score += 150;
            foreach (Bullet b in FindObjectsOfType<Bullet>()) Destroy(b.gameObject);
            Destroy(gameObject);
        }
    }
}
