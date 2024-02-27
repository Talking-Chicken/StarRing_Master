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
        base.Interact(player);
        feedbacks.PlayFeedbacks();


    }
    public void StopInteraction()
    {
        
        feedbacks.RestoreInitialValues();
        StopInteract();
        
    }
}
