using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class LaptopInteraction : Interactable
{
    [SerializeField] MMF_Player feedbacks;
    [SerializeField] TextMeshProUGUI photoTitle;
    protected override void Start()
    {
        base.Start();
       

    }
    private void Update()
    {
        if (photoTitle.text=="Magic Detecion Hardware"&& !MindPalaceManager.activeManager.GetNodeActive("ResearchNote")) 
        {
            StartDialogue("LaptopInvestiagtion");
        }
      
    }
    public override void Interact(PlayerProperty player)
    {
       
        if (!MindPalaceManager.activeManager.GetNodeActive("question_Rita_store"))
        {
            StartDialogue("talktoRita");
        }
        else {
            if (!MindPalaceManager.activeManager.GetNodeActive("ResearchNote"))
            {
                base.Interact(player);
                feedbacks.PlayFeedbacks();

            }
        }
    }
    protected override void OnDialogueCompleted()
    {
        if (!MindPalaceManager.activeManager.GetNodeActive("question_Rita_store"))
        {
            base.OnDialogueCompleted();
            StopInteract();
        }
    }
    public void StopInteraction()
    {
        
        feedbacks.RestoreInitialValues();
        StopInteract();
        
    }
}
