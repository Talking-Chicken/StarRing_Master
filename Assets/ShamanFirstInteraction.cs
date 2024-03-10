using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;
using Yarn.Unity;
using System;
using System.Reflection;


public class ShamanFirstInteraction : Interactable
{
    bool drifting;
    [SerializeField] MMF_Player feedbacks;
    [SerializeField] public shamanCameraControl shamanCamera;
    [SerializeField] LineView dialogueline;
    InMemoryVariableStorage variableStorage;
     void Start()
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
    }
    public override void Interact(PlayerProperty player)
    {
      
        base.Interact(player);
        StartDialogue("shaman");
        //  shamanCamera.transform.rotation = initialRotation;

        //    ChangeState(stateInvest);

    }
    private void Update()
    {
       
        bool testVariable;
        variableStorage.TryGetValue("$lookshaman", out testVariable);
        if (testVariable) {
          
                feedbacks.PlayFeedbacks();
                shamanCamera.Activied = true;
            ChangeState(stateInvest);
            // 获取LineView类型
            Type lineViewType = dialogueline.GetType();
                // 获取autoAdvance字段
                FieldInfo autoAdvanceField = lineViewType.GetField("autoAdvance", BindingFlags.NonPublic | BindingFlags.Instance);
                if (autoAdvanceField != null)
                {
                    // 读取autoAdvance的值
                    bool autoAdvanceValue = (bool)autoAdvanceField.GetValue(dialogueline);
                    Debug.Log("Current autoAdvance value: " + autoAdvanceValue);

                    // 修改autoAdvance的值
                    autoAdvanceField.SetValue(dialogueline, true);
                }
            }
          
                //dialogueline.holdTime = 2f;
             
            
           
        }
    
    public void StopInteraction()
    {
        shamanCamera.Activied = false;
        feedbacks.RestoreInitialValues();
        StopInteract();
        Cursor.lockState = CursorLockMode.None;
    }
    public void Interrupt()
    {
        
    }
    protected override void OnDialogueCompleted()
    {
        if (!MindPalaceManager.activeManager.GetNodeActive("question_Rita_store"))
        {
            base.OnDialogueCompleted();
            StopInteract();
        }
    }
}
