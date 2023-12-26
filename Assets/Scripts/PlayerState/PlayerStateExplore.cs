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
        player.HoveringInteractable = player.DetectInteractable();

        player.DetectInputExploreState();

        //when mouse is hovering over a new obj
        if ((player.HoveringInteractable != null && !player.HoveringInteractable.Equals(player.PreHoveringInteractable)) ||
            (player.PreHoveringInteractable != null) && !player.PreHoveringInteractable.Equals(player.HoveringInteractable)) {
            player.OnMouseStartHoverInterOBJ();
            player.OnMouseEndHoverInterOBJ();

            player.PreHoveringInteractable = player.HoveringInteractable;
        }

        // if (player.TargetInteractable != null && player.InteractionPosition != null) {
        //     Debug.Log("Woaaaaaa");
        //     if (player.IsReadyToInteract(player.InteractionPosition)) {
        //         Debug.Log("ready");
        //         player.RotateToward(player.TargetInteractable.transform);
        //         player.ChangeState(player.stateInteract);
        //         // player.Interact(player.TargetInteractable);
        //     }
        // }

        if (player.TargetInteractable != null) {
            Debug.Log("TARGET INTERACTABLE IS NOT NULL " + player.TargetInteractable.gameObject.name);
            player.ChangeState(player.stateInteract);
        }

        // if (player.TargetTalkingSetting != null) {
        //     if (player.IsReadyToTalk(player.TargetTalkingSetting.TalkingPosition)) {
        //         player.StartDialogue(player.TargetNPC);
        //     }
        // }
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
