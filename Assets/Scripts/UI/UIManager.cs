using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

public class UIManager : MMSingleton<UIManager>
{
    public SelectionMenu selectionMenu;
    public GameObject inventoryContainer, logicMapContainer, TimeManagementContainer;

    #region FSM
    private UIStateBase currentState;
    public UIStateBase previousState;
    public UIStateNone stateNone = new UIStateNone();
    public UIStatePause statePause = new UIStatePause();
    public UIStateSelecting stateSelecting = new UIStateSelecting();
    public UIStateInventory stateInventory = new UIStateInventory();
    public UIStateLogicMap stateLogicMap = new UIStateLogicMap();
    public UIStateTimeManagement stateTimeManagement = new UIStateTimeManagement();

    public void ChangeState(UIStateBase newState)
    {
        if (currentState != newState) {
            if (currentState != null)
            {
                currentState.LeaveState(this);
            }

            Debug.Log("changing from " + currentState + " to " + newState);
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

    #region Start and Update
    void Start()
    {
        currentState = stateNone;
        previousState = currentState;

        //add open ui function to selection menu's buttons' OnClick()
        
        selectionMenu.InventoryButton.onClick.AddListener(OpenInventory);
        selectionMenu.LogicMapButton.onClick.AddListener(OpenLogicMap);
        // selectionMenu.LogicMapButton.onClick.AddListener(OpenInventory);
        // selectionMenu.LogicMapButton.onClick.AddListener(OpenInventory);
    }
    
    void Update()
    {
        currentState.UpdateState(this);
        if (Input.GetKeyDown(KeyCode.P))
            GameManager.Instance.Pause(PauseMethods.NoPauseMenu);
    }
    #endregion

    public void OpenInventory() {
        ChangeState(stateInventory);
        inventoryContainer.SetActive(true);
    }

    public void CloseInventory() {
        ChangeState(stateNone);
        inventoryContainer.SetActive(false);
    }

    public void OpenLogicMap() {
        ChangeState(stateLogicMap);
        logicMapContainer.SetActive(true);
    }

    public void CloseLogicMap() {
        ChangeState(stateNone);
        logicMapContainer.SetActive(false);
    }

    public void OpenTimeManagement() {
        ChangeState(stateTimeManagement);
        TimeManagementContainer.SetActive(true);
    }

    public void CloseTimeManagement() {
        ChangeState(stateNone);
        TimeManagementContainer.SetActive(false);
    }
}
