using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : Investigation
{
    private FPSCameraControl cameraControl;
    [SerializeField] private string shortnodeName;
    [SerializeField] private string longNodeName;
    public override void Investigate(Interactable interactable)
    {
        base.Investigate(interactable);
        if (FPSCameraControl.isLongPressed)
            StartDialogue(longNodeName);
        else
            StartDialogue(shortnodeName);
        cameraControl = (interactable as Tent).tentCamera;
        cameraControl.canControl = false;
    }

    public override void StopInvestigate()
    {
        base.StopInvestigate();
        cameraControl.canControl = true;
    }

    protected override void OnDialogueCompleted()
    {
        base.OnDialogueCompleted();
        StopInvestigate();
    }
}
