using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.AI;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using Cinemachine;
using UnityEngine.SceneManagement;

public class YarnCommandManager : MonoBehaviour
{
    CharacterPathfinder3D characterAgent;
    CharacterOrientation3D characterOrient;
    Animator characterAnimator;
    IK_Manager IKManger;
    CinemachineBrain cinemachineBrain;
   

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

        characterAnimator.Play(clipName);
    }
    [YarnCommand("faceToward")]
    public void FaceToward(GameObject character, Transform destination)
    {
        
        characterOrient=character.GetComponent<CharacterOrientation3D>();
        characterOrient.CharacterRotationAuthorized=true;
        character.transform.LookAt(destination,Vector3.up);
    }

    [YarnCommand("nodeStatus")]
    public void NodeStatus(string nodeName,string tempvalue)
    {
       
        variableStorage.SetValue(tempvalue, MindPalaceManager.activeManager.GetNodeActive(nodeName));
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
        IKManger.target=target;
        IKManger.lookat = true;
        IKManger.OnAnimatorIK();
    }
    [YarnCommand("stoplookat")]
    public void StopLookAt(GameObject character)
    {
        IKManger = character.GetComponentInChildren<IK_Manager>();
        
        IKManger.lookat = false;
     
    }
    [YarnCommand("feedbackPlayer")]
    public void FeedbackPlayer(MMF_Player feedback)
    {
        feedback.PlayFeedbacks(); 
    }
    [YarnCommand("feedbackRestore")]
    public void FeedbackRestore(MMF_Player feedback)
    {
        feedback.RestoreInitialValues();
    }
    [YarnCommand("stopInteraction")]
    public void StopInteraction(string interactableName)
    {
        _interactableListener.stopInteract.Invoke(interactableName);
    }

    [YarnCommand("setPosition")]
    public void SetPosition(GameObject character, Transform newposition)
    {
        character.transform.position= newposition.position;
    }

   [YarnCommand("active")]
    public void Active(string gameObject, bool flag)
    {
     
        GameObject.Find(gameObject).SetActive(flag);
    }

    [YarnCommand("focous")]
    public void CameraFocous(GameObject gameObject)
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        cinemachineBrain.ActiveVirtualCamera.LookAt = gameObject.transform;
    }
    [YarnCommand("focousRest")]
    public void CameraFocousRest()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        cinemachineBrain.ActiveVirtualCamera.LookAt = null;
    }
    [YarnCommand("outline")]
    public void Outline(GameObject gameObject, bool outlineSwitch)
    {
        gameObject.GetComponent<AddOutline>().enableHighlight = outlineSwitch;
    }
    [YarnCommand("conditionINTUpdate")]
    public void ConditionINTUpdate(string conditionName, int newValue)
    {
        ConditionSystemManager.SetInt(conditionName, newValue);
    }

    [YarnCommand("Load")]
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    [YarnCommand("TentStopInteraction")]
    public void TentStopInteract(GameObject Interactable)
    {
        Interactable.GetComponent<Tent>().StopInteraction();
    }
    [YarnCommand("RabbitShow")]
    public void RabbitShow(GameObject MrRabbit)
    {
        StartCoroutine(ScaleUp(MrRabbit));

    }
     IEnumerator ScaleUp(GameObject MrRabbit)
    {
        float scaleTime = 0.3f; // 完成缩放的时间
        float currentTime = 0f;

        while (currentTime <= scaleTime)
        {
            float yScale = Mathf.Lerp(0, 0.3f, currentTime / scaleTime);
            MrRabbit.transform.localScale = new Vector3(0.3f, yScale, 0.3f);
            currentTime += Time.deltaTime;
            yield return null;
        }

        // 确保缩放到精确的1
        MrRabbit.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }
    [YarnCommand("RabbitDis")]
    public void RabbitDis(GameObject MrRabbit)
    {
        StartCoroutine(ScaleDown(MrRabbit));

    }
    IEnumerator ScaleDown(GameObject MrRabbit)
    {
        float scaleTime = 0.3f; // 完成缩放的时间
        float currentTime = 0f;

        while (currentTime <= scaleTime)
        {
            float yScale = Mathf.Lerp(0, 0, currentTime / scaleTime);
            MrRabbit.transform.localScale = new Vector3(0.3f, yScale, 0.3f);
            currentTime += Time.deltaTime;
            yield return null;
        }

        // 确保缩放到精确的1
        MrRabbit.transform.localScale = new Vector3(0.3f, 0, 0.3f);
    }
}

