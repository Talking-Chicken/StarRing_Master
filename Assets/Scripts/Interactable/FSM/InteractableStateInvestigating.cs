using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableStateInvestigating : InteractableStateBase
{
    public override void EnterState(Interactable interactable)
    {
        Debug.Log("Started Investigating");
    }
    public override void UpdateState(Interactable interactable)
    {
        interactable.InvestigatableUpdate();
    }
    public override void LeaveState(Interactable interactable) {
        Debug.Log("Ended Investigating");
    }
}
