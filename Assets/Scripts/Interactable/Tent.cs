using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;

public class Tent : Interactable
{
    [SerializeField] MMF_Player feedbacks;
    private Quaternion initialRotation;
    [SerializeField] public FPSCameraControl tentCamera;
    
    protected override void Start()
    {
        base.Start();
        initialRotation = tentCamera.transform.rotation; 

    }
    private void Update()
    {
        

    }
    public override void Interact(PlayerProperty player)
    {
        if (MindPalaceManager.activeManager.GetNodeActive("question_Rita_store"))
        {
            base.Interact(player);
            feedbacks.PlayFeedbacks();
            tentCamera.transform.rotation = initialRotation;
            tentCamera.Activied = true;
            ChangeState(stateInvest);
        }
        else 
        {
            StartDialogue("talktoRita");
        }
    }
    public void StopInteraction()
    {
        tentCamera.Activied = false;
        feedbacks.RestoreInitialValues();
        StopInteract();
        Cursor.lockState = CursorLockMode.None;
    }
    protected override void OnDialogueCompleted()
    {
        if (!MindPalaceManager.activeManager.GetNodeActive("question_Rita_store"))
        {
            base.OnDialogueCompleted();
            StopInteract();
        }
    }
}
