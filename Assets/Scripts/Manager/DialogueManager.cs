using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private DialogueRunner _dialogueRunner;
    [SerializeField, BoxGroup("References")] private LineView _lineView;
    [SerializeField, Foldout("Listeners")] private DialogueActionListener _dialogueListener;

    void OnEnable() {
        _dialogueListener.startDialogue.AddListener(StartDialogue);
        _dialogueListener.nextLine.AddListener(NextLine);
        _dialogueListener.stopDialogue.AddListener(StopDialogue);
    }

    void OnDisable() {
        _dialogueListener.startDialogue.RemoveListener(StartDialogue);
        _dialogueListener.nextLine.RemoveListener(NextLine);
        _dialogueListener.stopDialogue.RemoveListener(StopDialogue);
    }

    private void StartDialogue(string startNode) {
        _dialogueRunner.StartDialogue(startNode);
    }

    private void NextLine() {
        _lineView.OnContinueClicked();
    }

    public void SendDialogueCompleteEvent() {
        _dialogueListener.dialogueCompleted.Invoke();
    }

    private void StopDialogue()
    {
        if (_dialogueRunner.IsDialogueRunning)
            _dialogueRunner.Stop();
    }
}
