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
    [ShowNonSerializedField, BoxGroup("Info")] private CharacterPathfinder3D _characterPathFinder;
    [ShowNonSerializedField, BoxGroup("Info")] private CharacterMovement _characterMovement;
    [ShowNonSerializedField, BoxGroup("Info")] private MouseControls3D _mouseControl3D;
    [ShowNonSerializedField, BoxGroup("Info")] private DialogueRunner dialogueRunner;
    [ShowNonSerializedField, BoxGroup("Dialogue")] private NavMeshHit navHit;  

    [SerializeField, BoxGroup("test")] private GameObject testObj;
    private GameObject gg;


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
    }

    
    void Update()
    {
        currentState.UpdateState(this);

        
        if (Input.GetKeyDown(KeyCode.H)) {
            gg = Instantiate(testObj, transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            _characterPathFinder.SetNewDestination(gg.transform);
        }
    }

    public void limitMovement() {
        _characterPathFinder.SetNewDestination(transform);
        _mouseControl3D.AbilityPermitted = false;
    }

    public void releaseMovement() {
        _mouseControl3D.AbilityPermitted = true;
    }

}
