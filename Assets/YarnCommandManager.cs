using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.AI;
using MoreMountains.TopDownEngine;

public class YarnCommandManager : MonoBehaviour
{
    CharacterPathfinder3D characterAgent;
    Animator characterAnimator;
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
}
