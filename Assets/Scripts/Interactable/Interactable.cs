using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum InteractableType {OBJ, NPC, EXM}

/// this is the parent class for all interactable objects
public class Interactable : MonoBehaviour
{
    [SerializeField, BoxGroup("Cursor Settings")] private InteractableType type = InteractableType.OBJ;
    [SerializeField, BoxGroup("properties")] private string interactableName;
    [SerializeField, BoxGroup("Properties")] private bool isInteractable = true;
    [SerializeField, BoxGroup("Properties")] private List<Material> outlineMats;
    [SerializeField, BoxGroup("Interaction Settings"), Tooltip("whether this interactable needs update each frame")] private bool requiresUpdate = false;
    [SerializeField, BoxGroup("Interaction Settings")] private List<Transform> interactPositions;
    [SerializeField, Foldout("Listeners")] protected InteractableActionListener _interactListener;
    [SerializeField, Foldout("Listeners")] protected DialogueActionListener _dialogueListener;

    // getters & setters
    public InteractableType Type {get=>type; protected set=>type=value;}
    public bool IsInteractable {get=>isInteractable; set=>isInteractable=value;}
    public List<Material> OutlineMats {get=>outlineMats; protected set =>outlineMats=value;}
    
    protected virtual void Start() {}

    /// <summary>
    /// will run this function when requiresUpdate is true (substitution of regular Update method)
    /// </summary>
    public virtual void InteractableUpdate() {}

    public virtual void Interact(PlayerProperty player) {
        if (player == null)
            return;
        
        print("interacting with " + interactableName);
    }

    public virtual void Interact(PlayerProperty player, string interactableName) {
        if (!this.interactableName.Equals(interactableName))
            return;
        Interact(player);
    }

    public virtual void StopInteract() {
        _interactListener.stopInteract.Invoke(this);
    }

    public virtual void StartDialogue(string startNode) {
        _dialogueListener.startDialogue.Invoke(startNode);
    }
}
