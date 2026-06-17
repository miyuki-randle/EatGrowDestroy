using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour, IEventListener<GameEvent>
{
    public void Start()
    {
        EventBus.Instance.Subscribe(GameEvent.PlayerDeath, this);

        if (GetComponent<GroundObstacle>())
        {
            EventBus.Instance.Subscribe(GameEvent.StrengthActivate, this);
            EventBus.Instance.Subscribe(GameEvent.StrengthDeactivate, this);
        }
    }

    void Update()
    {
        if (transform.position.x < -35)
        {
            if (tag != "Food" && tag != "PowerUp" && tag != "Flying") GameManager.Instance.score += 100;
            Destroy(gameObject);
        }
    }

    public void OnEventRaised(GameEvent type)
    {
        switch(type)
        {
            case GameEvent.StrengthActivate:
                GetComponent<GroundObstacle>().breakable = true;
                GetComponent<GroundObstacle>().healthBar.gameObject.SetActive(true);
                break;
            case GameEvent.StrengthDeactivate:
                GetComponent<GroundObstacle>().breakable = false;
                GetComponent<GroundObstacle>().healthBar.gameObject.SetActive(false);
                break;
            case GameEvent.PlayerDeath:
                Destroy(gameObject);
                break;
        }
    }

    public void OnDestroy()
    {
        EventBus.Instance.Unsubscribe(GameEvent.PlayerDeath, this);

        if (GetComponent<GroundObstacle>())
        {
            EventBus.Instance.Unsubscribe(GameEvent.StrengthActivate, this);
            EventBus.Instance.Unsubscribe(GameEvent.StrengthDeactivate, this);
        }
    }
}
