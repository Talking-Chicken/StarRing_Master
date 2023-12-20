using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExploreStateBase
{
    public abstract void EnterState(PlayerManager player);
    public abstract void UpdateState(PlayerManager player);
    public abstract void LeaveState(PlayerManager player);
}
