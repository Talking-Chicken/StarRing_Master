using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Interactable Action Listener", menuName = "Star Ring/Listeners/Interactable Action Listener", order = 1)]
public class InteractableActionListener : ScriptableObject
{
    /// <summary>
    /// param: PlayerManager player, string target interactable name
    /// </summary>
    [System.NonSerialized]
    public UnityEvent<PlayerProperty, string> interact;

    /// <summary>
    /// param: Interactable target interactable
    /// </summary>
    [System.NonSerialized]
    public UnityEvent<Interactable> stopInteract;

    void OnEnable() {
        if (interact == null)
            interact = new UnityEvent<PlayerProperty, string>();
        if (stopInteract == null)
            stopInteract = new UnityEvent<Interactable>();
    }
}
