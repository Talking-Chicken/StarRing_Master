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

        //when mouse is hovering over a new obj
        if ((player.HoveringInteractable != null && !player.HoveringInteractable.Equals(player.PreHoveringInteractable)) ||
            (player.PreHoveringInteractable != null) && !player.PreHoveringInteractable.Equals(player.HoveringInteractable)) {
            player.OnMouseStartHoverInterOBJ();
            player.OnMouseEndHoverInterOBJ();

            player.PreHoveringInteractable = player.HoveringInteractable;
        }

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
        //clear outline
        if (player.HoveringInteractable != null) {
            player.PreHoveringInteractable = player.HoveringInteractable;
            player.OnMouseEndHoverInterOBJ();
        }

        player.previousState = this;
    }
}
