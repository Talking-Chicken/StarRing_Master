using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using NaughtyAttributes;

public class Investigation : MonoBehaviour
{
  public string interact_name;
  public Transform ui_location;
  protected Interactable parentInteractable = null; 

  /*[SerializeField] GameObject bubbleText;
  private bool once = true;
  DialogueRunner bubbleRunner;*/
  PlayerProperty player;
  // GameObject spawnedObject;
  [SerializeField, Foldout("Listeners")] private PlayerActionListener _playerListener;
  [SerializeField, Foldout("Listeners")] private DialogueActionListener _dialogueListener;

  #region FSM
  private InvestigatableStateBase currentState;
  public InvestigatableStateBase previousState;
  public InvestigatableStateIdle stateIdle = new InvestigatableStateIdle();
  public InvestigatableStateDialogue stateDialogue = new InvestigatableStateDialogue();

  public void ChangeState(InvestigatableStateBase newState)
  {
    if (currentState != newState)
    {
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

  public void ChangeToPreviousState()
  {
    if (currentState != previousState)
    {
      ChangeState(previousState);
    }
  }
  #endregion

  private void Start()
  {
    ChangeState(stateIdle);
    player = FindObjectOfType<PlayerProperty>();
  }

  public virtual void InvestigationUpdate()
  {
    currentState.UpdateState(this);
  }

  public virtual void Investigate(Interactable interactable)
  {
    print("Investigating " + interact_name + " by " + interactable.name);
    parentInteractable = interactable;
  }

  public virtual void StopInvestigate()
  {
    ChangeState(stateIdle);
    _playerListener.stopInvestigate.Invoke(this);
  }

  #region Dialogue
  public virtual void StartDialogue(string startNode)
  {
    _dialogueListener.startDialogue.Invoke(startNode);
    ChangeState(stateDialogue);
  }

  public virtual void StopDialogue()
  {
    _dialogueListener.stopDialogue.Invoke();
    ChangeState(stateIdle);
  }

  public virtual void NextDialogueLine()
  {
    _dialogueListener.nextLine.Invoke();
  }

  public void RegisterDialogueCompleteEvent()
  {
    _dialogueListener.dialogueCompleted.AddListener(OnDialogueCompleted);
  }

  public void UnRegisterDialogueCompleteEvent()
  {
    _dialogueListener.dialogueCompleted.RemoveListener(OnDialogueCompleted);
  }

  protected virtual void OnDialogueCompleted() { }
  #endregion

  public void InvestigationContent()
  {
    //  base.Interact(player);
    // StartDialogue(shortnodeName);
    //  ChangeState(stateDialogue);
  }

  public void ActualInvestigationContent()
  {
    // StartDialogue(longnodeName);
  }
}
