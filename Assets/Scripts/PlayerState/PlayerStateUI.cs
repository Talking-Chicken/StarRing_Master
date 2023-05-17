using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateUI : PlayerStateBase
{
    public override void EnterState(PlayerManager player)
    {
        // player.OpenUIFeedback.PlayFeedbacks();
        player.LimitMovement();
    }

    public override void UpdateState(PlayerManager player)
    {
        
    }

    public override void LeaveState(PlayerManager player)
    {
        player.previousState = this;
    }
}
