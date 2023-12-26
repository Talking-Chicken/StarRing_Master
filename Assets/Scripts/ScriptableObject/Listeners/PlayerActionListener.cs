using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Player Action Listener", menuName = "Star Ring/Listeners/Player Action Listener", order = 1)]
public class PlayerActionListener : ScriptableObject
{
    [System.NonSerialized]
    public UnityEvent<PlayerManager, Interactable> onInteractWithInteractable; //param: PlayerManager player, Interactable things that player interacting with
    [System.NonSerialized]
    public UnityEvent<Interactable> stopInteract; //param: Interactable the interactable that just stop interacting with

    void OnEnable() {
        if (onInteractWithInteractable == null)
            onInteractWithInteractable = new UnityEvent<PlayerManager, Interactable>();
        if (stopInteract == null)
            stopInteract = new UnityEvent<Interactable>();
    }
}
