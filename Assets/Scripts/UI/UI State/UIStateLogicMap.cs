using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateLogicMap : UIStateBase
{
    public override void EnterState(UIManager ui) {
        ui.SelectLogicMapFB.Direction = MoreMountains.Feedbacks.MMFeedbacks.Directions.TopToBottom;
        ui.SelectLogicMapFB.PlayFeedbacks();
        ui.OpenLogicMap();
    }
    public override void UpdateState(UIManager ui) {}
    public override void LeaveState(UIManager ui) {
        ui.SelectLogicMapFB.Direction = MoreMountains.Feedbacks.MMFeedbacks.Directions.BottomToTop;
        ui.SelectLogicMapFB.PlayFeedbacks();
        ui.CloseLogicMap();
        ui.previousState = this;
    }
}
