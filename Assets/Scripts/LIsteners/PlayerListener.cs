using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A Scriptable object listens all events that player does (like clicking on interactable objects, or the player character is created)
/// </summary>
[CreateAssetMenu(fileName = "Player Listener", menuName = "Star Ring/Listeners/PlayerListener", order = 2)]
public class PlayerListener : ScriptableObject
{
    [System.NonSerialized]
    public UnityEvent<PlayerManager> onCreatedEvent;
    [System.NonSerialized]
    public UnityEvent<Interactable> onMouseStartHoverInteractableEvent, onMouseEndHoverInteractableEvent;  

    private void OnEnable() {
        if (onCreatedEvent == null)
            onCreatedEvent = new UnityEvent<PlayerManager>();
        if (onMouseStartHoverInteractableEvent == null)
            onMouseStartHoverInteractableEvent = new UnityEvent<Interactable>();
        if (onMouseEndHoverInteractableEvent == null)
            onMouseEndHoverInteractableEvent = new UnityEvent<Interactable>();
    }

    public PlayerManager OnCreated(PlayerManager player) {
        if (player != null) {
            onCreatedEvent.Invoke(player);
            return player;
        }
        return null;
    }

    public Interactable OnMouseStartHoverInteractable(Interactable interactable) {
        if (interactable != null) {
            onMouseStartHoverInteractableEvent.Invoke(interactable);
            return interactable;
        }
        return null;
    }

    public Interactable OnMouseEndHoverInteractable(Interactable interactable) {
        if (interactable != null) {
            onMouseEndHoverInteractableEvent.Invoke(interactable);
            return interactable;
        }
        return null;
    }

}
