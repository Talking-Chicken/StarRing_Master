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
        //go to the point where Amo can talk
        if (player.TargetNPC !=null && player.TargetNPC.IsInteractable) {
            Transform targetTalkPosition = player.WalkToNearestTalkPosition(player.TargetNPC);
            if (targetTalkPosition != null) {
                if (player.isReadyToTalk(targetTalkPosition)) { //talk with character
                    player.StartDialogue(player.TargetNPC);
                }
            }
        }
        
    }

    public override void LeaveState(PlayerManager player)
    {
        player.previousState = this;
    }
}
