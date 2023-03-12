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
    [ReadOnly, SerializeField, BoxGroup("Info")] private CharacterPathfinder3D _characterPathFinder;
    [ReadOnly, SerializeField, BoxGroup("Info")] private CharacterMovement _characterMovement;
    [ReadOnly, SerializeField, BoxGroup("Info")] private MouseControls3D _mouseControl3D;
    [ReadOnly, SerializeField, BoxGroup("Info")] private DialogueRunner _dialogueRunner;
    [ReadOnly, SerializeField, BoxGroup("Dialogue")] private NavMeshHit navHit;
    [ReadOnly, SerializeField, BoxGroup("Dialogue")] private NPC targetNpc;
    [SerializeField, BoxGroup("Dialogue")] private GameObject targetTalkPosition;

    [SerializeField, BoxGroup("test")] private GameObject testObj;
    private GameObject gg;

    //getters & setters
    public NPC TargetNPC {get=>targetNpc; set=>targetNpc=value;}

    //FSM
    private PlayerStateBase currentState;
    public PlayerStateBase previousState;
    public PlayerStateExplore stateExplore = new PlayerStateExplore();
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
            if (currentState != null)
            {
                currentState.LeaveState(this);
            }

            currentState = previousState;

            if (currentState != null)
            {
                currentState.EnterState(this);
            }
        }
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
    }

    
    void Update()
    {
        currentState.UpdateState(this);

        
        if (Input.GetKeyDown(KeyCode.H)) {
            gg = Instantiate(testObj, transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            if (TargetNPC != null)
                WalkToNearestTalkPosition(TargetNPC);
        }
    }

    public void LimitMovement() {
        _characterPathFinder.SetNewDestination(transform);
        _mouseControl3D.AbilityPermitted = false;
    }

    public void ReleaseMovement() {
        _mouseControl3D.AbilityPermitted = true;
    }

    public Transform WalkToNearestTalkPosition(NPC npc) {
        NavMeshHit myNavHit;
        if(NavMesh.SamplePosition(npc.TalkingPositions[0].position, out myNavHit, 100 , -1))
        {
            GameObject talkingPosition = Instantiate(targetTalkPosition, myNavHit.position, Quaternion.identity);
            _characterPathFinder.SetNewDestination(talkingPosition.transform);
            return talkingPosition.transform;
        }
        return null;
    }

    public bool isReadyToTalk(Transform destination) {
        if (Vector3.Distance(transform.position, destination.position) <= 0.2f) {
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

}
