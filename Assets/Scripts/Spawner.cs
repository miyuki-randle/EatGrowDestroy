using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Spawner : MonoBehaviour, IEventListener<GameEvent>
{
    public float delay = 2.0f;
    public bool active = false;
    public Vector2 delayRange = new Vector2(2, 5);
    public float scaleFactor = 1f;

    // Start is called before the first frame update
    public void Start()
    {
        SubscribeToEvents();
        ResetDelay();
        StartCoroutine(ObjectGenerator());
    }

    void SubscribeToEvents()
    {
        if (!GetComponent<PowerUpSpawner>())
        {
            EventBus.Instance.Subscribe(GameEvent.PlayerGrowth, this);
            EventBus.Instance.Subscribe(GameEvent.PlayerShrink, this);
            EventBus.Instance.Subscribe(GameEvent.StageIncrease, this);

            if (GetComponent<GroundSpawner>())
            {
                EventBus.Instance.Subscribe(GameEvent.StrengthActivate, this);
                EventBus.Instance.Subscribe(GameEvent.StrengthDeactivate, this);
            }
        }

    }

    protected abstract IEnumerator ObjectGenerator();

    public void OnEventRaised(GameEvent type)
    {
        switch (type)
        {
            case GameEvent.PlayerGrowth:
                scaleFactor = 0.9f;
                break;
            case GameEvent.PlayerShrink:
                scaleFactor = 1.1f;
                break;
            case GameEvent.StageIncrease: // Reset scale and increase spawn rate for more difficulty
                scaleFactor = 1f;
                delay -= 0.5f;
                delayRange.x -= 0.5f;
                delayRange.y -= 1f;
                break;
            case GameEvent.StrengthActivate:
                GetComponent<GroundSpawner>().strengthActive = true;

                break;
            case GameEvent.StrengthDeactivate:
                GetComponent<GroundSpawner>().strengthActive = false;
                break;
        }
    }

    protected void ResetDelay()
    {
        delay = Random.Range(delayRange.x, delayRange.y);
    }
}
