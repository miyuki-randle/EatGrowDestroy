using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour, IEventListener<GameEvent>
{
    [SerializeField] private float speed = 7.0f;
    [SerializeField] private float lifetime = 2f;
    private Rigidbody rb;
    private Vector3 direction;
    private bool gameOver = false;

    void Start()
    {
        if (gameOver || !FindObjectOfType<PlayerController>().alive) { Destroy(gameObject); return; }
        EventBus.Instance.Subscribe(GameEvent.PlayerDeath, this);
        rb = GetComponent<Rigidbody>();
        direction = GameManager.Instance.player.transform.position - transform.position;
        direction.Normalize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direction * speed;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SizeHUD.Instance.ShrinkPlayer();
        }
        Destroy(gameObject);
    }

    public void OnEventRaised(GameEvent type)
    {
        Destroy(gameObject);
        gameOver = true;
    }

    private void OnDestroy()
    {
        EventBus.Instance.Unsubscribe(GameEvent.PlayerDeath, this);
    }
}
