using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalDevice : Interactable
{
    protected override void Start()
    {
        base.Start();
    }
    public override void Interact(PlayerProperty player)
    {
        base.Interact(player);
        if (MindPalaceManager.activeManager.GetNodeActive("question_Rita_store"))
        {
            StartDialogue("SignalDevice"); 
        }
        else {
            StartDialogue("talktoRita"); 
        }
            
        //  StopInteract();
    }
    protected override void OnDialogueCompleted()
    {
        base.OnDialogueCompleted();
        StopInteract();
    }
}