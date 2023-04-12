using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MoreMountains.TopDownEngine;
using Yarn.Unity;
using UnityEngine.AI;
using TopDownEngineExtensions;

public class PlayerManager : MonoBehaviour
{
    [ReadOnly, SerializeField, Foldout("Info")] private CharacterPathfinder3D _characterPathFinder;
    [ReadOnly, SerializeField, Foldout("Info")] private CharacterMovement _characterMovement;
    [ReadOnly, SerializeField, Foldout("Info")] private MouseControls3D _mouseControl3D;
    [ReadOnly, SerializeField, Foldout("Info")] private DialogueRunner _dialogueRunner;
    [ReadOnly, SerializeField, Foldout("Info")] private CharacterOrientation3D _characterOrientation;
    [ReadOnly, SerializeField, BoxGroup("Dialogue")] private NavMeshHit navHit;
    [ReadOnly, SerializeField, BoxGroup("Dialogue")] private NPC targetNpc;
    [ReadOnly, SerializeField, BoxGroup("Dialogue")] private TalkingSetting targetTalkingSetting;
    [SerializeField, BoxGroup("Dialogue")] private GameObject targetTalkPosition;
    [SerializeField, BoxGroup("Selection Menu")] private SelectionMenu SelectionMenu;

    [SerializeField, BoxGroup("test")] private GameObject testObj;
    [SerializeField, BoxGroup("Dialogue")] private GameObject virtualCamera;

    //getters & setters
    public NPC TargetNPC { get => targetNpc; set => targetNpc = value; }
    public TalkingSetting TargetTalkingSetting {get=>targetTalkingSetting; set=>targetTalkingSetting=value;}
    public GameObject VirtualCamera{get => virtualCamera; }

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
    void Awake() {
        if (SelectionMenu != null)
            UIManager.Instance.selectionMenu = SelectionMenu;
        else
            SelectionMenu = GetComponentInChildren<SelectionMenu>();
    }

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
        //go to mr rabbit state
        if (Input.GetMouseButtonDown(1)) {
            ChangeState(stateMrRabbit);
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

    public void LimiteMovementCompletely()
    {
        _characterPathFinder.ResetAbility();
        _mouseControl3D.ResetAbility();
        _characterPathFinder.Target = null;
        _characterPathFinder.AbilityPermitted = false;

    }

    public void ReleaseMovement() {
        _characterPathFinder.AbilityPermitted = true;
        _mouseControl3D.AbilityPermitted = true;
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
        Debug.Log(Vector3.Distance(transform.position, destination));
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
        UIManager.Instance.ChangeState(UIManager.Instance.stateSelecting);
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
