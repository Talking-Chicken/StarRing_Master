using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierDesk : InteractObj, IInteractable
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    /// interact with chashier desk,
    /// it makes player to go to the next time slot
    public override bool Interact() {
        base.Interact();
        Debug.Log("interacting with " + name);
        TimeManager.Instance.ChangeToNextState();
        return true;
    }
}
