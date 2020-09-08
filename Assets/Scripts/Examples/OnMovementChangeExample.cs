using Assets.Scripts.Events.Base;
using Assets.Scripts.Events.OnEvents;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMovementChangeExample : MonoBehaviour, ISubscriber
{
    public NotifierBase[] OnStartMovingWatchObjects;
    public NotifierBase[] OnStopMovingWatchObjects;

    // Start is called before the first frame update
    void Start()
    {
        foreach (INotifier notifier in OnStartMovingWatchObjects)
        {
            notifier.Subscribe(new OnStartMoving(null, null), this);
        }

        foreach (INotifier notifier in OnStartMovingWatchObjects)
        {
            notifier.Subscribe(new OnStopMoving(null, null), this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool OnNotify(IGameEvent gameEvent)
    {
        if (gameEvent is OnStartMoving)
        {
            OnStartMoving evt = (OnStartMoving)gameEvent;
            Debug.Log($"Target started moving!");
        }
        else if (gameEvent is OnStopMoving)
        {
            OnStopMoving evt = (OnStopMoving)gameEvent;
            Debug.Log($"Target stopped moving!");
        }
        return false;
    }
}
