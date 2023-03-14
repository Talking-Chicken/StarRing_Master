using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIStateBase
{
    public abstract void EnterState(UIManager ui);
    public abstract void UpdateState(UIManager ui);
    public abstract void LeaveState(UIManager ui);
}
