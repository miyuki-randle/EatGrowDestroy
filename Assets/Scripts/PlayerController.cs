using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IEventListener<GameEvent>
{
    public bool alive;
    public bool hasPowerUp;
    public float jump;
    [SerializeField] private float speed;
    [SerializeField] private float stomp;

    [SerializeField] private Collider playerCollider;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private ParticleSystemRenderer particleRender;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] clips;

    [SerializeField] private KeyCode shootKey;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootRef;

    private bool isJumping;
    private Vector3 origScale;

    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        particles = GetComponent<ParticleSystem>();
        particleRender = GetComponent<ParticleSystemRenderer>();
        source = GetComponent<AudioSource>();
        isJumping = false;
        origScale = transform.localScale;
        hasPowerUp = false;
        alive = true;
    }

    void SubscribeToEvents()
    {
        EventBus.Instance.Subscribe(GameEvent.PlayerGrowth, this);
        EventBus.Instance.Subscribe(GameEvent.StageIncrease, this);
        EventBus.Instance.Subscribe(GameEvent.PlayerShrink, this);
        EventBus.Instance.Subscribe(GameEvent.PlayerDeath, this);
    }

    void Update()
    {
        if (alive)
        {
            if (Input.GetKeyDown(shootKey))
            {
                anim.SetTrigger("ShootLaser");
                source.PlayOneShot(clips[2]);
                GameObject bullet = Instantiate(projectile, shootRef.position, projectile.transform.localRotation);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (alive)
        {
            HandleMovement();
        }

    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        anim.SetFloat("Speed", h);

        Vector3 move = this.transform.root.right * h;
        this.transform.root.position += move * speed * Time.deltaTime;

        Vector3 position = this.transform.root.position;

        // lose life if pushed off screen
        if (position.x < -20f)
        {
            SizeHUD.Instance.ShrinkPlayer();
            ResetPlayer(false);
        }

        // make sure player stays in screen and in position
        if (position.x > 11.2f) this.transform.root.position = new Vector3(11.2f, position.y, position.z);
        if (position.z != 0) this.transform.root.position = new Vector3(position.x, position.z, 0);

        // Jumping
        if (v > 0.1 && !isJumping)
        {
            isJumping = true;
            source.Stop();
            source.PlayOneShot(clips[0]);
            anim.SetTrigger("JumpPressed");
            //this.transform.root.position += new Vector3(0, v * jump * Time.deltaTime, 0);
            rb.velocity = Vector2.up * jump;
        }
        // stomping
        if (isJumping && v < 0.1)
        {
            rb.velocity += new Vector3(0, v * stomp * Time.deltaTime, 0);
        }
    }

    void ResetPlayer(bool playerStays)
    {
        // Destroy all other game objects
        MovingObject[] objs = GameObject.FindObjectsOfType<MovingObject>();
        
        foreach (MovingObject obj in objs)
        {
            Destroy(obj.gameObject);
        }

        // Reset player
        if (!playerStays)
        {
            anim.ResetTrigger("JumpPressed");
            anim.ResetTrigger("EatFood");
            anim.ResetTrigger("ShootLaser");
            transform.position = new Vector3(-10, -6.5f, 0);
            rb.velocity = Vector3.zero;
        }
    }

    public void OnEventRaised(GameEvent type)
    {
        switch (type)
        {
            case GameEvent.PlayerGrowth:
                transform.localScale *= 1.05f;
                break;
            case GameEvent.PlayerShrink:
                transform.localScale *= 0.95f;
                break;
            case GameEvent.StageIncrease:
                transform.localScale = origScale;
                ResetPlayer(true);
                break;
            case GameEvent.PlayerDeath:
                PlayerDie();
                break;
        }
    }

    void PlayerDie()
    {
        source.loop = false;
        source.Stop();
        source.PlayOneShot(clips[3]);
        rb.velocity = Vector3.zero;
        alive = false;
        anim.SetBool("IsDead", true);
        ResetPlayer(true);
        //Destroy(this.gameObject);

    }

    private void OnDestroy()
    {
        EventBus.Instance.Unsubscribe(GameEvent.PlayerGrowth, this);
        EventBus.Instance.Unsubscribe(GameEvent.StageIncrease, this);
        EventBus.Instance.Unsubscribe(GameEvent.PlayerShrink, this);
        EventBus.Instance.Unsubscribe(GameEvent.PlayerDeath, this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isJumping && (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Ground"))
        {
            isJumping = false;
            source.PlayOneShot(clips[1]);
            source.Play();
        }
    }

    public void ActivatePowerUp(PowerUp obj)
    {
        if (!hasPowerUp)
        {
            StartCoroutine(obj.power);
        }
        Destroy(obj.gameObject);
    }

    // Power Ups
    public IEnumerator LifePower()
    {
        LifeHUD.Instance.HealPlayer();
        yield return null;
    }

    public IEnumerator StrengthPower()
    {
        hasPowerUp = true;
        particleRender.material.color = Color.blue;
        particles.Play();
        EventBus.Instance.DispatchEvent(GameEvent.StrengthActivate);

        yield return new WaitForSeconds(15);
        EventBus.Instance.DispatchEvent(GameEvent.StrengthDeactivate);
        particleRender.material.color = Color.red;
        particles.Stop();
        hasPowerUp = false;
    }

    public IEnumerator JumpPower()
    {
        float origJump = jump;
        jump = origJump + 5;
        particleRender.material.color = Color.green;
        particles.Play();
        hasPowerUp = true;

        yield return new WaitForSeconds(15);
        jump = origJump;
        particles.Stop();
        hasPowerUp = false;
    }
}
