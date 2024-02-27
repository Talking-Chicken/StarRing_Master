using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitaInteractionNightTent : Interactable
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
            if (MindPalaceManager.activeManager.GetNodeActive("repair_service") || MindPalaceManager.activeManager.GetNodeActive("magic_research")|| MindPalaceManager.activeManager.GetNodeActive("you_are_a_spy") || MindPalaceManager.activeManager.GetNodeActive("refund_soda"))
            {
                StartDialogue("Ritabeforesolution");
            }
            else 
            {
                StartDialogue("Ritahavesolution");
            }
        }
        else {
            StartDialogue("RitaNight");
        }
       
        //  StopInteract();
    }
    protected override void OnDialogueCompleted()
    {
        base.OnDialogueCompleted();
        StopInteract();
    }
}
