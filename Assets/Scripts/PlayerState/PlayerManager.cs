using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MoreMountains.TopDownEngine;
using Yarn.Unity;

public class PlayerManager : MonoBehaviour
{
    [ShowNonSerializedField, BoxGroup("Info")] private CharacterPathfinder3D _characterPathFinder;
    [ShowNonSerializedField, BoxGroup("Info")] private CharacterMovement _characterMovement;
    [ShowNonSerializedField, BoxGroup("Info")] private DialogueRunner dialogueRunner;


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
    }

    
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void limitMovement() {
        _characterMovement.AbilityPermitted = false;
        _characterPathFinder.SetNewDestination(transform);
    }

    public void releaseMovement() {
        _characterMovement.AbilityPermitted = true;
    }
}
