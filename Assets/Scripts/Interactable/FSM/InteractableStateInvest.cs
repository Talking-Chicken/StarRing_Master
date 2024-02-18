using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableStateInvest : InteractableStateBase
{
    public override void EnterState(Interactable interactable)
    {
        Debug.Log("Started Investigation");
    }
    public override void UpdateState(Interactable interactable)
    {
        if (Input.GetMouseButtonDown(0))
        {
            interactable.NextDialogueLine();
        }
    }
    public override void LeaveState(Interactable interactable) { }
}
