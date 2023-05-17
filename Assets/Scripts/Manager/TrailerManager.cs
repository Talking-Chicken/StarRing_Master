using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using MoreMountains.Feedbacks;

public class TrailerManager : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    [SerializeField] private LineView lineView;
    [SerializeField] private MMF_Player solutionAppearFB;
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            solutionAppearFB.PlayFeedbacks();
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            dialogueRunner.StartDialogue("Trailer_LogicMap");
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            lineView.OnContinueClicked();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            solutionAppearFB.PlayFeedbacksInReverse();
        }
    }
}
