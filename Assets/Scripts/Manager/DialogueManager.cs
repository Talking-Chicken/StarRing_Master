using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private DialogueRunner _dialogueRunner;
    [SerializeField, Foldout("Listeners")] private DialogueActionListener _dialogueListener;

    void OnEnable() {
        _dialogueListener.startDialogue.AddListener(StartDialogue);
    }

    void OnDisable() {
        _dialogueListener.startDialogue.RemoveListener(StartDialogue);
    }

    private void StartDialogue(string startNode) {
        _dialogueRunner.StartDialogue(startNode);
    }
}
