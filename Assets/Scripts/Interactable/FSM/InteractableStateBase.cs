using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableStateBase
{
    public abstract void EnterState(Interactable interactable);
    public abstract void UpdateState(Interactable interactable);
    public abstract void LeaveState(Interactable interactable);
}
