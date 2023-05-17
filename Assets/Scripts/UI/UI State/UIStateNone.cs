using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateNone : UIStateBase
{
    public override void EnterState(UIManager ui) {
        ui.topBarButtons.SetActive(false);
        ui.topBarContainer.SetActive(false);
    }
    public override void UpdateState(UIManager ui) {}
    public override void LeaveState(UIManager ui) {
        ui.topBarContainer.SetActive(true);
        ui.previousState = this;
    }
}
