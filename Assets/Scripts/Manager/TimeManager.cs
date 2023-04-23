using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;

public class TimeManager : MMSingleton<TimeManager> 
{
    #region FSM
    private TimeStateBase currentState;
    public TimeStateBase previousState, nextState;
    public TimeStateEvening stateEvening = new TimeStateEvening();
    public TimeStateNight stateNight = new TimeStateNight();
    public TimeStateLateNight stateLateNight = new TimeStateLateNight();
    public TimeStateTransition stateTransition = new TimeStateTransition();

    public void ChangeState(TimeStateBase newState)
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

    public void ChangeToNextState() {
        if (currentState != nextState) {
            ChangeState(nextState);
        }
    }
    #endregion

    void Start()
    {
        currentState = stateEvening;
        nextState = stateNight;
        previousState = stateEvening;
    }

    
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SceneTransitionTo(string sceneName) {
        SceneManager.LoadScene(sceneName);
        Debug.Log("transited scene");
    }
}
