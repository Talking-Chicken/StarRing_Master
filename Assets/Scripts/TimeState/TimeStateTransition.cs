using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStateTransition : TimeStateBase
{
    public override void EnterState(TimeManager TimeManager)
    {
    }

    public override void UpdateState(TimeManager TimeManager)
    {
    }

    public override void LeaveState(TimeManager TimeManager)
    {
        TimeManager.previousState = this;
    }
}
