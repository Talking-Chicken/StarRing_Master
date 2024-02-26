using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;

public class InteractableStateDialogue : InteractableStateBase
{
    public override void EnterState(Interactable interactable){
        interactable.RegisterDialogueCompleteEvent();
        Debug.Log("Started dialogue");
    }
    public override void UpdateState(Interactable interactable){
        if (Input.GetMouseButtonDown(0)) {
            interactable.NextDialogueLine();
        }
    }
    public override void LeaveState(Interactable interactable){
        interactable.UnRegisterDialogueCompleteEvent();
    }
}
