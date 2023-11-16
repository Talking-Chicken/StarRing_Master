using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateInteract : PlayerStateBase
{    
    public override void EnterState(PlayerManager player)
    {
        player.LimitMovement();
        if (player.TargetInteractable == null)
            player.ChangeState(player.stateExplore);
        
        player.TargetInteractable.Interact(player.Property);
    }

    public override void UpdateState(PlayerManager player)
    {
        
    }

    public override void LeaveState(PlayerManager player)
    {
        player.previousState = this;
    }
}
