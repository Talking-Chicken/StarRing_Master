using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    #region FSM
    private UIStateBase currentState;
    public UIStateBase previousState;
    public UIStateNone stateNone = new UIStateNone();
    public UIStatePause statePause = new UIStatePause();
    public UIStateSelecting stateSelecting = new UIStateSelecting();
    public UIStateInventory stateInventory = new UIStateInventory();
    public UIStateLogicMap stateLogicMap = new UIStateLogicMap();

    public void ChangeState(UIStateBase newState)
    {
        if (currentState != newState) {
            if (currentState != null)
            {
                currentState.LeaveState(this);
            }

            currentState = newState;

            if (currentState != null)
            {
                currentState.EnterState(this);
            }
        }
    }

    public void ChangeToPreviousState() {
        if (currentState != previousState) {
            ChangeState(previousState);
        }
    }
    #endregion

    void Start()
    {
        currentState = stateNone;
        previousState = currentState;
    }
    
    void Update()
    {
        currentState.UpdateState(this);
    }
}
