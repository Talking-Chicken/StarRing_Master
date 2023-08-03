using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A Scriptable object listens all events that dealing with UI. 
/// </summary>
[CreateAssetMenu(fileName = "UI Listener", menuName = "Star Ring/Listeners/UIListener", order = 1)]
public class UIListener : ScriptableObject
{
    [System.NonSerialized]
    public UnityEvent openRabbitEvent, closeRabbitEvent;
    [System.NonSerialized]
    public UnityEvent<UIManager> onCreatedEvent;
    [System.NonSerialized]
    public UnityEvent onStartDialogue;

    private void OnEnable() {
        if (openRabbitEvent == null)
            openRabbitEvent = new UnityEvent();
        if (closeRabbitEvent == null)
            closeRabbitEvent = new UnityEvent();
        if (onCreatedEvent == null)
            onCreatedEvent = new UnityEvent<UIManager>();
    }

    public UIManager OnCreated(UIManager ui) {
        if (ui != null) {
            onCreatedEvent.Invoke(ui);
            return ui;
        }
        return null;
    }

    public bool OpenRabbitUI() {
        openRabbitEvent.Invoke();
        return true;
    }

    public bool closeRabbitUI() {
        closeRabbitEvent.Invoke();
        return true;
    }
}
