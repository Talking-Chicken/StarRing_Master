using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMrRabbit : PlayerStateBase
{
    public override void EnterState(PlayerManager player)
    {
        UIManager.Instance.ChangeState(UIManager.Instance.stateSelecting);
        player.OpenSelectionMenu();
    }

    public override void UpdateState(PlayerManager player)
    {
        player.DetectInputMrRabbitState();
    }

    public override void LeaveState(PlayerManager player)
    {
        player.CloseSelectionMenu();
        player.previousState = this;
    }
}
