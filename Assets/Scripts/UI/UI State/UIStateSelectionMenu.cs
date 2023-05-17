using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateSelectionMenu : UIStateBase
{
    public override void EnterState(UIManager ui)
    {
        ui.topBarButtons.SetActive(true);
        ui.OpenSelectionMenu();
    }

    public override void UpdateState(UIManager ui)
    {
        
    }

    public override void LeaveState(UIManager ui)
    {   
        ui.CloseSelectionMenu();
        ui.previousState = this;
    }
}
