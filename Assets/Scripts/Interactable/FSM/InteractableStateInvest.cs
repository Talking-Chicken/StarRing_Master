using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableStateInvest : InteractableStateBase
{
    private float cd = 0.5f;
    public override void EnterState(Interactable interactable)
    {
        cd = 0.5f;
    }
    public override void UpdateState(Interactable interactable)
    {
        if (cd >= 0)
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
