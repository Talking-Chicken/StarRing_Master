using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MoreMountains.TopDownEngine;
using Yarn.Unity;
using UnityEngine.AI;
using TopDownEngineExtensions;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class PlayerManager : MonoBehaviour
{
    //info
    [ReadOnly, SerializeField, Foldout("Info")] private CharacterPathfinder3D _characterPathFinder;
    [ReadOnly, SerializeField, Foldout("Info")] private CharacterMovement _characterMovement;
    [ReadOnly, SerializeField, Foldout("Info")] private MouseControls3D _mouseControl3D;
    [ReadOnly, SerializeField, Foldout("Info")] private DialogueRunner _dialogueRunner;
    [ReadOnly, SerializeField, Foldout("Info")] private CharacterOrientation3D _characterOrientation;
    private Camera _mainCamera;

    //interactions
    [ReadOnly, SerializeField, BoxGroup("Interaction")] private Interactable hoveringInteractable;
    [ReadOnly, SerializeField, BoxGroup("Interaction")] private Interactable targetInteractable;
    [ReadOnly, SerializeField, BoxGroup("Interaction")] private Transform interactionPosition;
    [ReadOnly, SerializeField, BoxGroup("Interaction")] private bool isInteracting = false;
    [SerializeField, BoxGroup("Interaction")] private LayerMask interactionRaycastMask;

    //Dialogues
    [ReadOnly, SerializeField, BoxGroup("Dialogue")] private NavMeshHit navHit;
    [ReadOnly, SerializeField, BoxGroup("Dialogue")] private NPC targetNpc;
    [ReadOnly, SerializeField, BoxGroup("Dialogue")] private TalkingSetting targetTalkingSetting;
    [SerializeField, BoxGroup("Dialogue")] private GameObject targetTalkPosition;
    [SerializeField, BoxGroup("Dialogue")] private GameObject virtualCamera;

    //selection menu
    [SerializeField, BoxGroup("Selection Menu")] private SelectionMenu SelectionMenu;

    [SerializeField, BoxGroup("test")] private GameObject testObj;
    
    //feedbacks
    [SerializeField, Foldout("Feedbacks")] private MMF_Player openUIFeedback;

    #region getters & setters
    public Interactable HoveringInteractable {get=>hoveringInteractable;set=>hoveringInteractable=value;}
    public Interactable TargetInteractable {get=>targetInteractable;private set=>targetInteractable=value;}
    public Transform InteractionPosition {get=>interactionPosition;private set=>interactionPosition=value;}
    public bool IsInteracting {get=>isInteracting;private set=>isInteracting=value;}
    public NPC TargetNPC { get => targetNpc; set => targetNpc = value; }
    public TalkingSetting TargetTalkingSetting {get=>targetTalkingSetting; set=>targetTalkingSetting=value;}
    public GameObject VirtualCamera{get => virtualCamera;}
    public MMF_Player OpenUIFeedback {get=>openUIFeedback;}
    #endregion

    #region FSM
    private PlayerStateBase currentState;
    public PlayerStateBase previousState;
    public PlayerStateExplore stateExplore = new PlayerStateExplore();
    public PlayerStateMrRabbit stateMrRabbit = new PlayerStateMrRabbit();
    public PlayerStateDialogue stateDialogue = new PlayerStateDialogue();
    public PlayerStateUI stateUI = new PlayerStateUI();

    public void ChangeState(PlayerStateBase newState)
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

    #region Awake, Start, and Update
    void Awake() {}

    void Start()
    {
        currentState = stateExplore;
        previousState = currentState;

        //set up
        _characterPathFinder = GetComponent<CharacterPathfinder3D>();
        if (_characterPathFinder == null)
            Debug.LogWarning("Can't find CharacterPathfinder3D");

        _characterMovement = GetComponent<CharacterMovement>();
        if (_characterMovement == null)
            Debug.LogWarning("Can't find CharacterMovement");

        _mouseControl3D = GetComponent<MouseControls3D>();
        if (_mouseControl3D == null)
            Debug.LogWarning("Can't find MouseControl3D");

        _dialogueRunner = FindObjectOfType<DialogueRunner>();
        if (_dialogueRunner == null)
            Debug.LogWarning("Can't find DialogueRunner");

        _characterOrientation = GetComponent<CharacterOrientation3D>();
        if (_characterOrientation == null)
            Debug.LogWarning("Can't find Character Orientation");
        
        _mainCamera = Camera.main;

        //add exit dialogue state function to dialogue runner's OnDialogueComplete event
        _dialogueRunner.onDialogueComplete.AddListener(ChangeToPreviousState);
    }

    
    void Update()
    {
        currentState.UpdateState(this);

        if (Input.GetKeyDown(KeyCode.J)) {
            if (TargetNPC != null)
                WalkToNearestTalkPosition(TargetNPC);
        }
    }
    #endregion

    #region detect input
    public void DetectInputExploreState() {
        //detect interact
        if (Input.GetMouseButtonUp(0)) {
            if (HoveringInteractable != null) {
                TargetInteractable = HoveringInteractable;
                switch (TargetInteractable.Type) {
                    case InteractableType.OBJ:
                        InteractObj obj = TargetInteractable.GetComponent<InteractObj>();
                        InteractionPosition = obj.InteractPosition;
                        WalkToInteractingPosition(obj.InteractPosition);
                        // (targetInteractable as InteractObj).Interact();
                        break;
                    case InteractableType.EXM:
                        break;
                    case InteractableType.NPC:
                        break;
                }
            }
        }

        //go to UI state
        if (Input.GetMouseButtonDown(1)) {
            ChangeState(stateUI);
        }
    }

    public void DetectInputMrRabbitState() {
        //go to explore state
        if (Input.GetMouseButtonDown(1)) {
            ChangeState(stateExplore);
        }
    }
    #endregion

    /*restrict the ability to use mouse control 3d*/
    public void LimitMovement() {
        _characterPathFinder.SetNewDestination(transform);
        _mouseControl3D.AbilityPermitted = false;
    }

    public void ReleaseMovement() {
        _characterPathFinder.AbilityPermitted = true;
        _mouseControl3D.AbilityPermitted = true;
    }

    public void ResetInteractTargets() {
        TargetInteractable = null;
        InteractionPosition = null;
        IsInteracting = false;
    }

    /// rotate player character to face target transform's position
    public void RotateToward(Transform target) {
        if (target == null) {
            Debug.Log("rotation toward target is null");
            return;
        }
        Vector3 targetAngle = new Vector3(target.position.x -target.position.x, 0, target.position.z - target.position.z).normalized;
        PlayerFace(new Vector2(targetAngle.x,targetAngle.z));
    }
 
    /// ray cast from camera to mouse world position,
    /// it detects if anything is interactable
    public Interactable DetectInteractable() {
        if (MMGUI.PointOrTouchBlockedByUI()) return null;

        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
#if UNITY_EDITOR
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue);
#endif
        Interactable interactable = null;
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, interactionRaycastMask))
        {
            hitInfo.transform.TryGetComponent<Interactable>(out interactable);
        }
        return interactable;
    }

    /// interact with target
    /// do corresponding things based on their type
    public void Interact(Interactable target) {
        if (target == null)
            return;
        
        switch (target.Type) {
            case InteractableType.OBJ:
                InteractObj obj = TargetInteractable.GetComponent<InteractObj>();
                obj.Interact();
                break;
            case InteractableType.EXM:
                break;
            case InteractableType.NPC:
                break;
        }
        IsInteracting = true;
    }

    /// go to the interacting position
    public void WalkToInteractingPosition(Transform interactingPosition) {
        _characterPathFinder.SetNewDestination(interactingPosition);
    }

    /// return whether player is close enough to interact
    public bool IsReadyToInteract(Transform destination) {
        if (Vector3.Distance(transform.position, destination.position) <= 0.25f) {
            return true;
        } else {
            return false;
        }
    }

    /*compare all talking positions of the NPC
      return the nearrest transform*/
    public TalkingSetting FindNearestTalkSetting(NPC npc) {
        if (npc.TalkingSettings.Count <= 0)
            return null;
        
        Vector3 nearestPosition = transform.position;
        TalkingSetting nearestTalkingSetting = null;
        float minDistance = -1.0f;
        for (int i = 0; i < npc.TalkingSettings.Count; i++) {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, npc.TalkingSettings[i].TalkingPosition, NavMesh.AllAreas, path);

            //get the total distance of the path
            float distance = 0.0f;
            for (int j = 0; j < path.corners.Length - 1; j++)
                distance += Vector3.Distance(path.corners[j], path.corners[j + 1]);

            //compare minDistance with the current path distance
            if (minDistance < 0 || minDistance > distance) {
                minDistance = distance;
                nearestPosition = npc.TalkingSettings[i].TalkingPosition;
                nearestTalkingSetting = npc.TalkingSettings[i];
            }
        }
        return TargetTalkingSetting = nearestTalkingSetting;
    }

    /*nave mesh find the npc's nearest talking position (all talking positions are inside NPC)
      if there's no talking point in the scene, instantiate one
      if there's one, change the position of it*/
    public Transform WalkToNearestTalkPosition(NPC npc) {
        NavMeshHit myNavHit;
        FindNearestTalkSetting(npc);
        Vector3 targetPosition = TargetTalkingSetting.TalkingPosition;

        if (TargetTalkingSetting == null)
            return null;
        if (targetPosition == null)
            return null;

        if(NavMesh.SamplePosition(targetPosition, out myNavHit, 100 , -1))
        {
            GameObject talkingPosition = GameObject.Find("TalkingPosition");
            if (talkingPosition == null) {
                talkingPosition = Instantiate(targetTalkPosition, myNavHit.position, Quaternion.identity);
                talkingPosition.name = "TalkingPosition";
            } else
                talkingPosition.transform.position = myNavHit.position;
            _characterPathFinder.SetNewDestination(talkingPosition.transform);
            return talkingPosition.transform;
        }
        return null;
    }

    public bool IsReadyToTalk(Transform destination) {
        if (Vector3.Distance(transform.position, destination.position) <= 0.25f) {
            return true;
        } else {
            return false;
        }
    }

    public bool IsReadyToTalk(Vector3 destination) {
        if (Vector3.Distance(transform.position, destination) <= 0.25f) {
            return true;
        } else {
            return false;
        }
    }

    public bool StartDialogue(NPC npc) {
        if (!_dialogueRunner.IsDialogueRunning) {
            _dialogueRunner.StartDialogue(npc.StartNodeBase + "_" + (npc.GetProgress(npc.StartNodeBase)+1));
            ChangeState(stateDialogue);
            return true;
        }
        return false;
    }

    public void OpenSelectionMenu() {
        SelectionMenu.gameObject.SetActive(true);
        SelectionMenu.CurrentOptionIndex = 1;
    }

    public void CloseSelectionMenu() {
        
        SelectionMenu.gameObject.SetActive(false);
        SelectionMenu.CurrentOptionIndex = 1;
    }

    public void PlayerFace(Vector2 angle) {
        _characterMovement.SetMovement(angle);
    }
}
