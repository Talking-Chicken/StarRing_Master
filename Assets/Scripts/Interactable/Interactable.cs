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
    [SerializeField, BoxGroup("Investigation")] protected Investigation interactingInvestigation = null, detectingInvestigation = null;
    [BoxGroup("Investigation")] protected Ray mouseRay;
    [BoxGroup("Investigation")] protected RaycastHit mouseHit;
    [SerializeField, Foldout("Listeners")] protected InteractableActionListener _interactListener;
    [SerializeField, Foldout("Listeners")] protected DialogueActionListener _dialogueListener;
    [SerializeField, Foldout("Listeners")] protected PlayerActionListener _playerListener;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private string currentStateName;

    // getters & setters
    public InteractableType Type {get=>type; protected set=>type=value;}
    public bool IsInteractable {get=>isInteractable; set=>isInteractable=value;}
    public List<Material> OutlineMats {get=>outlineMats; protected set =>outlineMats=value;}

    #region FSM
    private InteractableStateBase currentState;
    public InteractableStateBase previousState;
    public InteractableStateIdle stateIdle = new InteractableStateIdle();
    public InteractableStateDialogue stateDialogue = new InteractableStateDialogue();
    public InteractableStateInvest stateInvest = new InteractableStateInvest();
    public InteractableStateInvestigating stateInvestigating = new InteractableStateInvestigating();

    public void ChangeState(InteractableStateBase newState)
    {
        if (currentState != newState) {
            if (currentState != null)
            {
                currentState.LeaveState(this);
            }

            previousState = currentState;
            currentState = newState;
            currentStateName = currentState.ToString();

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
        _playerListener.stopInvestigate.AddListener(StopInvestigate);
    }

    protected virtual void OnDisable() {
        _interactListener.interact.RemoveListener(Interact);
        _interactListener.stopInteract.RemoveListener(StopInteract);
        _playerListener.stopInvestigate.RemoveListener(StopInvestigate);
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

    #region Interact
    public virtual void Interact(PlayerProperty player) {
        if (player == null)
            return;
        
        _playerListener.rotateToward.Invoke(this.transform);
        
        print("interacting with " + interactableName);
    }

    private void Interact(PlayerProperty player, string interactableName) {
        if (!this.interactableName.Equals(interactableName))
            return;
        Interact(player);
    }

    protected virtual void StopInteract() {
        StopDialogue();
        _playerListener.stopInteract.Invoke(this);
    }

    private void StopInteract(string interactableName) {
        if (!interactableName.Equals(this.interactableName, System.StringComparison.OrdinalIgnoreCase))
            return;
        StopInteract();
    }
    #endregion

    #region Dialogue
    public virtual void StartDialogue(string startNode) {
        _dialogueListener.startDialogue.Invoke(startNode);
        ChangeState(stateDialogue);
    }

    public virtual void StopDialogue() {
        _dialogueListener.stopDialogue.Invoke();    
        ChangeState(stateIdle);
    }

    public void RegisterDialogueCompleteEvent()
    {
        _dialogueListener.dialogueCompleted.AddListener(OnDialogueCompleted);
    }

    public void UnRegisterDialogueCompleteEvent()
    {
        _dialogueListener.dialogueCompleted.RemoveListener(OnDialogueCompleted);
    }

    protected virtual void OnDialogueCompleted() {}

    public virtual void NextDialogueLine() {
        _dialogueListener.nextLine.Invoke();
    }
    #endregion

    #region Invest
    public void DetectInvestigatable()
    {
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out mouseHit))
        {
            if (mouseHit.transform.TryGetComponent<Investigation>(out detectingInvestigation))
            {
                Investigate(detectingInvestigation);
            }
        }
    }

    protected void Investigate(Investigation targetInvestigation)
    {
        targetInvestigation.Investigate(this);
        interactingInvestigation = detectingInvestigation;
        ChangeState(stateInvestigating);
    }

    public void InvestigatableUpdate()
    {
        interactingInvestigation.InvestigationUpdate();
    }

    protected void StopInvestigate(Investigation targetInvestigation)
    {
        if (interactingInvestigation != targetInvestigation)
            return;
        
        ChangeState(stateInvest);
    }
    #endregion

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
