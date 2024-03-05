using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : Interactable
{
    protected override void Start()
    {
        base.Start();
    }
    public override void Interact(PlayerProperty player)
    {
        base.Interact(player);
        // _dialogueListener.startDialogue.Invoke("Argument1");
        StartDialogue("NoCampingSign");
        //  StopInteract();
    }
    protected override void OnDialogueCompleted()
    {
        base.OnDialogueCompleted();
        StopInteract();
    }
}
