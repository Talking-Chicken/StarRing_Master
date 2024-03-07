using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : Interactable
{
    public string nodeName;
    protected override void Start()
    {
        base.Start();
    }
    public override void Interact(PlayerProperty player)
    {
        base.Interact(player);
        // _dialogueListener.startDialogue.Invoke("Argument1");
        StartDialogue(nodeName);
        //  StopInteract();
    }
}