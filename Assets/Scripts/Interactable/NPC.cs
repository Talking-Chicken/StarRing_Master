using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Yarn.Unity;
using Cinemachine;

public class NPC : MonoBehaviour, ITalkable
{
    [ReadOnly, SerializeField, BoxGroup("Dialogue")] private bool isInteractable = true;
    [SerializeField, BoxGroup("Dialogue")] private string startNodeBase;
    [ShowNonSerializedField, BoxGroup("Dialogue")] private Dictionary<string,int> interactCount = new Dictionary<string, int>();
    [SerializeField, BoxGroup("Dialogue")] private List<Transform> talkingPositions;

    //getters & setters
    public bool IsInteractable {get=>isInteractable;set=>isInteractable=value;}
    public string StartNodeBase {get=>startNodeBase;set=>startNodeBase=value;}
    public List<Transform> TalkingPositions {get=>talkingPositions; private set=>talkingPositions=value;}

    #region FSM
    private NPCStateBase currentState;
    public NPCStateBase previousState;
    public NPCStateTransition stateTransition = new NPCStateTransition();
    public NPCStateExecution stateExecution = new NPCStateExecution();

    public void ChangeState(NPCStateBase newState)
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
    
    void Start() {
        currentState = stateExecution;
        previousState = currentState;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.G))
            Debug.Log(interactCount["Helmet_1"]);
        
        currentState.UpdateState(this);
    }

    //ITalkable
    [YarnCommand("SetProgress")]
    public void SetProgress(string nodeBase) {
        if (interactCount.ContainsKey(nodeBase)) {
            interactCount[nodeBase]++;
        } else {
            interactCount.Add(nodeBase, 1);
        }
    }

    public int GetProgress(string nodeBase) {
        if (interactCount.ContainsKey(nodeBase))
            return interactCount[nodeBase];
        else
            return 0;
    }

    /*return the closest position from a transform*/
    public void getClosestPosition(Transform target) {
        
    }
}
