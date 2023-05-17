using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateInventory : UIStateBase
{
    public override void EnterState(UIManager ui) {
        ui.SelectInventoryFB.PlayFeedbacks();
        ui.OpenInventory();
    }
    public override void UpdateState(UIManager ui) {}
    public override void LeaveState(UIManager ui) {
        ui.SelectInventoryFB.PlayFeedbacksInReverse();
        ui.CloseInventory();
        ui.previousState = this;
    }
}
