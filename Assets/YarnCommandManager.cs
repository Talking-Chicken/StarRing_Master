using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.AI;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;
using NaughtyAttributes;


public class YarnCommandManager : MonoBehaviour
{
    CharacterPathfinder3D characterAgent;
    Animator characterAnimator;
    IK_Manager IKManger;
    [SerializeField, Foldout("Listeners")] private InteractableActionListener _interactableListener;
    private InMemoryVariableStorage variableStorage;
    public void Start()
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
    }
    [YarnCommand("walk")]
    public void Walk(Transform destination,GameObject character)
    {
        characterAgent = character.GetComponent<CharacterPathfinder3D>();

        characterAgent.SetNewDestination(destination);
    }
    [YarnCommand("animation")]
    public void Animation(GameObject character,string clipName)
    {
        characterAnimator = character.GetComponentInChildren<Animator>();
        Debug.Log(characterAnimator);

        characterAnimator.Play(clipName);
    }
    [YarnCommand("faceToward")]
    public void FaceToward(Transform destination, GameObject character)
    {
        character.transform.LookAt(destination,Vector3.up);
    }
    [YarnCommand("nodeStatus")]
    public void NodeStatus(string nodeName)
    {
       
        variableStorage.SetValue("$tempBool", MindPalaceManager.activeManager.GetNodeActive(nodeName));
    }
    [YarnCommand("activeNode")]
    public void ActiveNode(string nodeName)
    {
        MindPalaceManager.activeManager.ActiveNode(nodeName);
    }
    [YarnCommand("lookat")]
    public void LookAt(GameObject character, GameObject target)
    {
        IKManger = character.GetComponentInChildren<IK_Manager>();

        IKManger.OnAnimatorIK(target);
    }
    [YarnCommand("feedbackPlayer")]
    public void FeedbackPlayer(MMF_Player feedback)
    {
        feedback.PlayFeedbacks(); 
    }
    [YarnCommand("stopInteraction")]
    public void StopInteraction(string interactableName)
    {
        _interactableListener.stopInteract.Invoke(interactableName);
    }

}
