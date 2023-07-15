using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using NaughtyAttributes;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [BoxGroup("UI References"), SerializeField] private UIListener _UIListener; 
    [BoxGroup("UI References")] public GameObject selectionMenuContainer, inventoryContainer, logicMapContainer, TimeManagementContainer, topBarContainer, topBarButtons;
    [SerializeField, BoxGroup("UI References")] private GameObject blackOverlay;
    public CanvasGroup logicMapCanvasGroup;
    [ReadOnly, SerializeField, Foldout("Other References")] private PlayerManager _playerManager;
    [SerializeField, Foldout("Feedbacks")] private MMF_Player selectSettingFB, selectInventoryFB, selectLogicMapFB, selectionMenuFBIN, selectionMenuFBOUT;

    #region getters & setters
    public MMF_Player SelectSettingFB {get=>selectSettingFB;}
    public MMF_Player SelectInventoryFB {get=>selectInventoryFB;}
    public MMF_Player SelectLogicMapFB {get=>selectLogicMapFB;}
    public MMF_Player SelectionMenuFBIN {get=>selectionMenuFBIN;}
    public MMF_Player SelectionMenuFBOUT {get=>selectionMenuFBOUT;}
    #endregion

    #region FSM
    private UIStateBase currentState;
    public UIStateBase previousState;
    public UIStateNone stateNone = new UIStateNone();
    public UIStatePause statePause = new UIStatePause();
    public UIStateSelectionMenu stateSelectionMenu = new UIStateSelectionMenu();
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

    ///used for button OnClick()
    public void ChangeToInventoryState() {ChangeState(stateInventory);}
    public void ChangeToLogicMapState() {ChangeState(stateLogicMap);}
    #endregion

    #region Start, Update, onEnable...
    void OnEnable() {
        _UIListener.OnCreated(this);
    }

    void OnDisable() {
        _UIListener.openRabbitEvent.RemoveAllListeners();
        _UIListener.closeRabbitEvent.RemoveAllListeners();
    }

    void Start()
    {
        ///set references
        _playerManager = FindObjectOfType<PlayerManager>();
        if (_playerManager == null)
            Debug.LogWarning("Can't find Player Manager in " + name);   

        currentState = stateNone;
        previousState = currentState;

        logicMapCanvasGroup.alpha = 0;
        logicMapCanvasGroup.interactable = false;
        logicMapCanvasGroup.blocksRaycasts = false;

        ///add open ui function to selection menu's buttons' OnClick()
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

    ///this will make player change back to explore state and close the black overlay
    public void CloseUI() {
        blackOverlay.SetActive(false);
        _playerManager.ChangeState(_playerManager.stateExplore);
        ChangeState(stateNone);
    }

    ///open Rabbit UI Menu (the menu that allows player to select inventory/logic map/system settings)
    public void OpenSelectionMenu() {
        _UIListener.OpenRabbitUI();
        selectionMenuContainer.SetActive(true);
    }

    ///close Rabbit UI Menu
    public void CloseSelectionMenu() {
        _UIListener.closeRabbitUI();
        selectionMenuContainer.SetActive(false);
    }

    public void OpenInventory() {
        blackOverlay.SetActive(true);
        inventoryContainer.SetActive(true);
    }

    public void CloseInventory() {
        inventoryContainer.SetActive(false);
    }

    public void OpenLogicMap() {
        blackOverlay.SetActive(true);
        logicMapCanvasGroup.alpha = 1;
        logicMapCanvasGroup.interactable = true;
        logicMapCanvasGroup.blocksRaycasts = true;
    }

    public void CloseLogicMap() {
        logicMapCanvasGroup.alpha = 0;
        logicMapCanvasGroup.interactable = false;
        logicMapCanvasGroup.blocksRaycasts = false;
    }
}
