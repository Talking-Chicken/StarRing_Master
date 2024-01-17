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
    [SerializeField, Foldout("Listeners")] private InteractableActionListener _interactableListener;
  
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
        MindPalaceManager.activeManager.GetNodeActive(nodeName);
    }
    [YarnCommand("activeNode")]
    public void ActiveNode(string nodeName)
    {
        MindPalaceManager.activeManager.ActiveNode(nodeName);
    }
    [YarnCommand("feedbackPlayer")]
    public void FeedbackPlayer(MMF_Player feedback)
    {
        feedback.PlayFeedbacks(); 
    }
    [YarnCommand("stopInteraction")]
    public void stopInteraction(string interactableName)
    {
        _interactableListener.stopInteract.Invoke(interactableName);
    }

}
