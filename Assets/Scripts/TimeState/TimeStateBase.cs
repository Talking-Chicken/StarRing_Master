using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeStateBase
{
    public abstract void EnterState(TimeManager timeManager);
    public abstract void UpdateState(TimeManager timeManager);
    public abstract void LeaveState(TimeManager timeManager);
}