using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateExplore : PlayerStateBase
{
    public override void EnterState(PlayerManager player)
    {
        player.ReleaseMovement();
        player.ResetInteractTargets();
    }

    public override void UpdateState(PlayerManager player)
    {
        player.HoveringInteractable = player.DetectInteractable();

        player.DetectInputExploreState();

        if (!player.IsInteracting) {
            if (player.TargetInteractable != null && player.InteractionPosition != null) {
                if (player.IsReadyToInteract(player.InteractionPosition)) {
                    player.RotateToward(player.TargetInteractable.transform);
                    player.Interact(player.TargetInteractable);
                }
            }
        }

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
