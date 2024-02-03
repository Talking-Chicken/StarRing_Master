using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitaInteractionAfternoon : Interactable
{
    protected override void Start()
    {
        base.Start();
    }
    public override void Interact(PlayerProperty player)
    {
        print("AAAAAAAAAAAAAAAAAAAA");
        base.Interact(player);
        print("BBBBBBBBBBBBBBBBBBBB");
        // _dialogueListener.startDialogue.Invoke("Argument1");
        StartDialogue("Argument1");
        //  StopInteract();
    }
}
