using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InvestigatableStateDialogue : InvestigatableStateBase
{
    public override void EnterState(Investigation investigation) {}
    public override void UpdateState(Investigation investigation) 
    {
        if (Input.GetMouseButtonDown(0))
        {
            investigation.NextDialogueLine();
        }
    }
    public override void LeaveState(Investigation investigation) {}
}
