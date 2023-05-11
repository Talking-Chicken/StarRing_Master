using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateSetting : UIStateBase
{
    public override void EnterState(UIManager ui)
    {
        ui.SelectSettingFB.PlayFeedbacks();
    }

    public override void UpdateState(UIManager ui)
    {
        
    }

    public override void LeaveState(UIManager ui)
    {
        ui.SelectSettingFB.PlayFeedbacksInReverse();
        ui.previousState = this;
    }
}
