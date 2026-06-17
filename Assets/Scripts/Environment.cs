using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour, IEventListener<GameEvent>
{
    public float speed;
    [SerializeField] private Renderer render;
    [SerializeField] private AudioSource aud;
    [SerializeField] private Material[] textures;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private GameObject transition;

    private bool gameRunning = false;

    void Awake()
    {
        render = GetComponent<Renderer>();
        aud = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventBus.Instance.Subscribe(GameEvent.GameStart, this);
        EventBus.Instance.Subscribe(GameEvent.StageIncrease, this);
        EventBus.Instance.Subscribe(GameEvent.PlayerDeath, this);
    }

    void Update()
    {
        // scrolling background
        if (gameRunning) render.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }

    public void OnEventRaised(GameEvent type)
    {
        switch (type)
        {
            case GameEvent.GameStart:
                gameRunning = true;
                if (GetComponent<AudioSource>()) aud.Play();
                break;
            case GameEvent.PlayerDeath:
                gameRunning = false;
                if (GetComponent<AudioSource>()) aud.Stop();
                break;
            case GameEvent.StageIncrease:
                StartCoroutine(ChangeStage());
                break;
        }

    }

    IEnumerator ChangeStage()
    {
        if (GetComponent<AudioSource>())
        {
            Instantiate(transition, transition.transform.position, transition.transform.rotation);
            aud.clip = sounds[GameManager.Instance.stage];
        }
        yield return new WaitForSeconds(2);
        render.material = textures[GameManager.Instance.stage];

        if (tag == "Platform") transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
        if (tag == "Background") transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
    }
}
