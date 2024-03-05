using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MindPalaceNode))]
public class MindPalaceNodeListener : MonoBehaviour
{
    private int draggingInvoked;
    public MindPalaceDraggingConditionCheck[] draggingEventList;
    public UnityEvent[] onactiveEvent;

    public float DraggingTimer
    {
        get; private set;
    }
    public void SetDraggingTimer(float timer)
    {
        if (timer == 0)
        {
            draggingInvoked = 0;
            foreach (MindPalaceDraggingConditionCheck check in draggingEventList)
            {
                check.Reset();
            }
        }
        DraggingTimer = timer;
    }

    MindPalaceNode _node;
    public MindPalaceNode MindPalaceNode
    {
        get
        {
            if (!_node)
                _node = GetComponent<MindPalaceNode>();
            return _node;
        }
        set => _node = value;
    }
    public void InvokeActiveEvent()
    {
        foreach (UnityEvent activeEvent in onactiveEvent)
        {
            activeEvent.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(MindPalaceNode.ID);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (MindPalaceDraggingConditionCheck check in draggingEventList)
        {
            if (check.CheckCondition(DraggingTimer))
            {
                draggingInvoked++;
            }
        }
    }
}
[Serializable]
public class MindPalaceConditionCheck
{
    [SerializeField] public string name;
    [SerializeField] public UnityEvent invokeEvent;
    [SerializeField] public ConditionSet conditionSet;
    [SerializeField] public MindPalaceNodeCheckSet nodeSet;
    public virtual void Reset()
    {

    }
    protected virtual bool CheckCondition()
    {
        if (conditionSet != null && !conditionSet.CheckCondition())
        {
            return false;
        }
        if (nodeSet != null && !conditionSet.CheckCondition())
        {
            return false;
        }
        invokeEvent.Invoke();
        return true;
    }
}



[Serializable]
public class MindPalaceDraggingConditionCheck : MindPalaceConditionCheck
{
    [Range(0.1f, 30f)]
    [SerializeField] public float draggingTime = .2f;
    private bool draggingInvoked;
    public bool CheckCondition(float timer)
    {
        if (draggingInvoked)
        {
            return false;
        }
        if (timer < draggingTime)
        {
            return false;
        }
        draggingInvoked = true;
        return base.CheckCondition();
    }
    public override void Reset()
    {
        base.Reset();
        draggingInvoked = false;
    }
}
[Serializable]
public class MindPalaceNodeCheckSet
{
    [SerializeField] public MindPalaceNodeCheck[] nodeChecks;
    [HideInInspector]
    public bool Check
    {
        get
        {
            foreach (MindPalaceNodeCheck check in nodeChecks)
            {
                if (!check.Check)
                {
                    return false;
                }
            }
            return true;
        }
    }

    [Serializable]
    public class MindPalaceNodeCheck
    {
        [SerializeField] public string name;
        [SerializeField] public bool active;
        private MindPalaceNode _node;
        private MindPalaceNode Node
        {
            get
            {
                if (!_node)
                    if (MindPalaceManager.activeManager.TryGetNode(name, out MindPalaceNode node))
                    {
                        _node = node;
                    }
                    else
                    {
                        Debug.LogError(name + " does not exist");
                    }
                return _node;
            }
        }
        [HideInInspector] public bool Check => Node.gameObject.activeSelf;
    }
}