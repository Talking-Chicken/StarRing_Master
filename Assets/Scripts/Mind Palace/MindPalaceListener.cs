using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MindPalaceManager))]
public class MindPalaceListener : MonoBehaviour
{
    MindPalaceManager _manager;

    [HideInInspector]public bool dragging = false;
    public float triggerIntervalTime = 15;
    private int invokedTimes;
    public UnityEvent idleEvent;
    public MindPalaceManager Manager
    {
        get
        {
            if (!_manager)
                _manager = GetComponent<MindPalaceManager>();
            return _manager;
        }
        set => _manager = value;
    }

    public float IdleTime
    {
        private set; get;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            IdleTime = 0;
            invokedTimes = 0;
        }
        else
        {
            IdleTime += Time.deltaTime;
            if (IdleTime > triggerIntervalTime * (invokedTimes + 1))
            {
                idleEvent.Invoke();
                invokedTimes++;
            }
        }
    }
}
