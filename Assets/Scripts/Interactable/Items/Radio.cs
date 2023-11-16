using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : InteractableObj
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Interact(PlayerProperty player)
    {
        base.Interact(player);
        StopInteract();
    }
}
