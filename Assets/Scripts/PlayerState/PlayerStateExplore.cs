using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateExplore : PlayerStateBase
{
    public override void EnterState(PlayerManager player)
    {
        player.ReleaseMovement();
    }

    public override void UpdateState(PlayerManager player)
    {
        player.DetectInputExploreState();

        if (player.TargetTalkingSetting != null) {
            if (player.IsReadyToTalk(player.TargetTalkingSetting.TalkingPosition)) {
                player.StartDialogue(player.TargetNPC);
            }
        }
    }

    public override void LeaveState(PlayerManager player)
    {
        player.previousState = this;
    }
}
