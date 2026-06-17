using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance;
    private Dictionary<GameEvent, List<IEventListener<GameEvent>>> listeners;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
        listeners = new Dictionary<GameEvent, List<IEventListener<GameEvent>>>();
    }

    public void Subscribe(GameEvent eventType, IEventListener<GameEvent> listener)
    {
        if (listeners.ContainsKey(eventType))
        {
            listeners[eventType].Add(listener);
        } else
        {
            var newList = new List<IEventListener<GameEvent>> { listener };
            listeners.Add(eventType, newList);
        }
    }

    public void Unsubscribe(GameEvent eventType, IEventListener<GameEvent> listener)
    {
        if (listeners.ContainsKey(eventType))
        {
            if (listeners[eventType].Contains(listener))
            {
                listeners[eventType].Remove(listener);
            }
        }
    }

    public void DispatchEvent(GameEvent type)
    {
        if (!listeners.ContainsKey(type)) return;
        foreach (var listener in listeners[type])
        {
            listener.OnEventRaised(type);
        }
    }
}

public interface IEventListener<T>
{
    public void OnEventRaised(T type);
}

public enum GameEvent
{
    GameStart,
    PlayerGrowth,
    StageIncrease,
    PlayerShrink,
    PlayerDeath,
    StrengthActivate,
    StrengthDeactivate,
}