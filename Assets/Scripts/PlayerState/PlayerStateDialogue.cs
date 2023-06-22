using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
public class PlayerStateDialogue : PlayerStateBase
{
    public override void EnterState(PlayerManager player)
    {
        player.LimitMovement();

        //set camera zoom in
        MMF_CinemachineTransition cinemachineTransition = DialogueAudio.Instance.zoomIn.GetFeedbackOfType<MMF_CinemachineTransition>();
        // cinemachineTransition.Channel = player.TargetNPC.TalkingSettings
        DialogueAudio.Instance.ZoomIn();


        NPCFace.Instance.FaceEachOther();
    }

    public override void UpdateState(PlayerManager player)
    {
        player.EnsureLineView();
        player.ContinueDialogue();
    }

    public override void LeaveState(PlayerManager player)
    {
        player.TargetNPC = null;
        player.previousState = this;
        DialogueAudio.Instance.ZoomOut();

    }
}
