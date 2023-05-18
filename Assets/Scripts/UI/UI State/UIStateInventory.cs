using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateInventory : UIStateBase
{
    public override void EnterState(UIManager ui) {
        ui.SelectInventoryFB.Direction = MoreMountains.Feedbacks.MMFeedbacks.Directions.TopToBottom;
        ui.SelectInventoryFB.PlayFeedbacks();
        ui.OpenInventory();
    }
    public override void UpdateState(UIManager ui) {}
    public override void LeaveState(UIManager ui) {
        ui.SelectInventoryFB.Direction = MoreMountains.Feedbacks.MMFeedbacks.Directions.BottomToTop;
        ui.SelectInventoryFB.PlayFeedbacks();
        ui.CloseInventory();
        ui.previousState = this;
    }
}
