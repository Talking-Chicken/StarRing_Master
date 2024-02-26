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
    public override void Interact(PlayerProperty player)
    {
        base.Interact(player);
        feedbacks.PlayFeedbacks();
        tentCamera.transform.rotation = initialRotation;
        tentCamera.Activied = true;
        ChangeState(stateInvest);
    }
}
