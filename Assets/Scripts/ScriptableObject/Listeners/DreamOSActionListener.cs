using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DreamOS Action Listener", menuName = "Star Ring/Listeners/DreamOS Action Listener", order = 1)]
public class DreamOSActionListener : ScriptableObject
{
    [System.NonSerialized]
    public UnityEvent openUI;
    [System.NonSerialized]
    public UnityEvent closeUI;

    void OnEnable()
    {
        if (openUI == null)
            openUI = new UnityEvent();
        if (closeUI == null)
            closeUI = new UnityEvent();
    }
}
