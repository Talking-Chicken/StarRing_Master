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
    [SerializeField, BoxGroup("Interaction Settings"), Tooltip("player needs to move closer than this number to start interact")] private float minInteractDistance = 0.2f;
    [SerializeField, Foldout("Listeners")] protected InteractableActionListener _interactListener;
    [SerializeField, Foldout("Listeners")] protected DialogueActionListener _dialogueListener;
    [SerializeField, Foldout("Listeners")] protected PlayerActionListener _playerListener;

    // getters & setters
    public InteractableType Type {get=>type; protected set=>type=value;}
    public bool IsInteractable {get=>isInteractable; set=>isInteractable=value;}
    public List<Material> OutlineMats {get=>outlineMats; protected set =>outlineMats=value;}

    #region FSM
    private InteractableStateBase currentState;
    public InteractableStateBase previousState;
    public InteractableStateIdle stateIdle = new InteractableStateIdle();
    public InteractableStateDialogue stateDialogue = new InteractableStateDialogue();

    public void ChangeState(InteractableStateBase newState)
    {
        if (currentState != newState) {
            if (currentState != null)
            {
                currentState.LeaveState(this);
            }

            previousState = currentState;
            currentState = newState;

            if (currentState != null)
            {
                currentState.EnterState(this);
            }
        }
    }

    public void ChangeToPreviousState() {
        if (currentState != previousState) {
            ChangeState(previousState);
        }
    }
    #endregion

    protected virtual void OnEnable() {
        _interactListener.interact.AddListener(Interact);
        _interactListener.stopInteract.AddListener(StopInteract);
        _dialogueListener.dialogueCompleted.AddListener(OnDialogueCompleted);
    }

    protected virtual void OnDisable() {
        _interactListener.interact.RemoveListener(Interact);
        _interactListener.stopInteract.RemoveListener(StopInteract);
        _dialogueListener.dialogueCompleted.RemoveListener(OnDialogueCompleted);
    }

    protected virtual void Start() {
        ChangeState(stateIdle);
    }

    /// <summary>
    /// will run this function when requiresUpdate is true (substitution of regular Update method)
    /// </summary>
    public virtual void InteractableUpdate() {
        currentState.UpdateState(this);
    }

    /// <summary>
    /// move to interactable position (if havent), then interact
    /// </summary>
    /// <param name="player">player property of the player that is trying to interact</param>
    public virtual void TryInteract(PlayerProperty player)
    {
        if (Vector3.Distance(player.transform.position, transform.position) > minInteractDistance)
        {
            
        }
    }

    public virtual void Interact(PlayerProperty player) {
        if (player == null)
            return;
        
        print("interacting with " + interactableName);
    }

    private void Interact(PlayerProperty player, string interactableName) {
        if (!this.interactableName.Equals(interactableName))
            return;
        Interact(player);
    }

    public virtual void StopInteract() {
        StopDialogue();
        _playerListener.stopInteract.Invoke(this);
    }

    private void StopInteract(string interactableName) {
        if (!interactableName.Equals(this.interactableName, System.StringComparison.OrdinalIgnoreCase))
            return;
        StopInteract();
    }

    public virtual void StartDialogue(string startNode) {
        _dialogueListener.startDialogue.Invoke(startNode);
        ChangeState(stateDialogue);
    }

    public virtual void StopDialogue() {
        _dialogueListener.stopDialogue.Invoke();    
        ChangeState(stateIdle);
    }

    protected virtual void OnDialogueCompleted() {}

    public virtual void NextDialogueLine() {
        _dialogueListener.nextLine.Invoke();
    }

    public virtual Transform GetNearestInteractingPoint(Transform interactor)
    {
        if (interactPositions.Count <= 0)
            return transform;

        Transform nearest = interactPositions[0];
        foreach (Transform transform in interactPositions)
            if (Vector3.Distance(interactor.transform.position, transform.position) < Vector3.Distance(interactor.transform.position, nearest.transform.position))
                nearest = transform;
        return nearest;
    }
}
