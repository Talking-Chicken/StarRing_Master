using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class PlayerStateDialogue : PlayerStateBase
{
   
    public override void EnterState(PlayerManager player)
    {
       
        player.LimitMovement();
        DialogueAudio.Instance.ZoomIn();
        NPCFace.Instance.FaceEachOther();
    }

    public override void UpdateState(PlayerManager player)
    {
        
    }

    public override void LeaveState(PlayerManager player)
    {
        player.TargetNPC = null;
        player.previousState = this;
        DialogueAudio.Instance.ZoomOut();

    }
}
