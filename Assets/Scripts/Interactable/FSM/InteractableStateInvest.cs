using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableStateInvest : InteractableStateBase
{
    private float cd = 3;
    public override void EnterState(Interactable interactable)
    {
        cd = 3;
    }
    public override void UpdateState(Interactable interactable)
    {
        if (cd >= 3)
        {
            cd -= Time.deltaTime;
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                interactable.DetectInvestigatable();
            }
        }
    }
    public override void LeaveState(Interactable interactable) {}
}
