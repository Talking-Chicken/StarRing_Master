using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using NaughtyAttributes;

public class DialogueUIManager : MonoBehaviour
{
    [SerializeField, BoxGroup("Listeners")] private DialogueListener _dialogueListener;
    [SerializeField, BoxGroup("References")] private DialogueRunner _dialogueRunner;
    [SerializeField, BoxGroup("References")] private LineView _lineView;
    [SerializeField, BoxGroup("References")] private CanvasGroup _dialogueCanvasGroup;

    //getters & setters
    public DialogueRunner DialogueRunner {get=>_dialogueRunner;}
    public LineView LineView {get=>_lineView;}

    private void OnEnable() {
        _dialogueListener.OnCreated(this);
    }

    void Start() {
    }

    public void HideDialogueBox() {
        _dialogueCanvasGroup.alpha = 0;
    }

    public void ShowDialogueBox() {
        _dialogueCanvasGroup.alpha = 1;
    }
}
