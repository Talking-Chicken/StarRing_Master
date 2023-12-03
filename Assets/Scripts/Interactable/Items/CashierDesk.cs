using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierDesk : InteractableObj, IInteractable
{
    protected override void Start()
    {
        base.Start();
    }

    /// interact with chashier desk,
    /// it makes player to go to the next time slot
    public override void Interact(PlayerProperty player) {
        base.Interact(player);
        TimeManager.Instance.ChangeToNextState();
    }
}
