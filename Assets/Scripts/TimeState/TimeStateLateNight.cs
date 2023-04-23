using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStateLateNight : TimeStateBase
{
    public override void EnterState(TimeManager TimeManager)
    {
        TimeManager.SceneTransitionTo("Main-Late Night");
    }

    public override void UpdateState(TimeManager TimeManager)
    {
    }

    public override void LeaveState(TimeManager TimeManager)
    {
        TimeManager.previousState = this;
    }
}
