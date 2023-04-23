using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStateNight : TimeStateBase
{
    public override void EnterState(TimeManager TimeManager)
    {
        TimeManager.SceneTransitionTo("Main - Night");
        TimeManager.nextState = TimeManager.stateLateNight;
    }

    public override void UpdateState(TimeManager TimeManager)
    {
    }

    public override void LeaveState(TimeManager TimeManager)
    {
        TimeManager.previousState = this;
    }
}
