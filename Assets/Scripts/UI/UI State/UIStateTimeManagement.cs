using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateTimeManagement : UIStateBase
{
    public override void EnterState(UIManager ui) {
        ui.OpenTimeManagement();
    }
    public override void UpdateState(UIManager ui) {}
    public override void LeaveState(UIManager ui) {
        ui.previousState = this;
    }
}
