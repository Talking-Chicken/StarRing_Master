using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStateEvening : TimeStateBase
{
    public override void EnterState(TimeManager TimeManager)
    {
        TimeManager.SceneTransitionTo("Main - Sunset");
        TimeManager.nextState = TimeManager.stateNight;
    }

    public override void UpdateState(TimeManager TimeManager)
    {
    }

    public override void LeaveState(TimeManager TimeManager)
    {
        TimeManager.previousState = this;
    }
}
