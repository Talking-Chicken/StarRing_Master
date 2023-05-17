using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateLogicMap : UIStateBase
{
    public override void EnterState(UIManager ui) {
        ui.SelectLogicMapFB.PlayFeedbacks();
        ui.OpenLogicMap();
    }
    public override void UpdateState(UIManager ui) {}
    public override void LeaveState(UIManager ui) {
        ui.SelectLogicMapFB.PlayFeedbacksInReverse();
        ui.CloseLogicMap();
        ui.previousState = this;
    }
}
