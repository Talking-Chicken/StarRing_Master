using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InvestigatableStateBase
{
    public abstract void EnterState(Investigation investigation);
    public abstract void UpdateState(Investigation investigation);
    public abstract void LeaveState(Investigation investigation);
}
