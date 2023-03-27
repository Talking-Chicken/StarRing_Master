using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class PlayerStateDialogue : PlayerStateBase
{
     MMF_Player zoom_in= GameObject.Find("MMF Zoom_in").transform.GetComponent<MMF_Player>();
    MMF_Player zoom_out= GameObject.Find("MMF Zoom_out").transform.GetComponent<MMF_Player>();
    public override void EnterState(PlayerManager player)
    {
        
        player.LimitMovement();
        zoom_in.PlayFeedbacks();
    }

    public override void UpdateState(PlayerManager player)
    {
        
    }

    public override void LeaveState(PlayerManager player)
    {
        player.TargetNPC = null;
        player.previousState = this;
        zoom_out.PlayFeedbacks();
    }
}
