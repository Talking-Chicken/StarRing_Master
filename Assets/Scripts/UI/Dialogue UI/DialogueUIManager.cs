using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueUIManager : MonoBehaviour
{
    private LineView _lineView;
    [SerializeField] private CanvasGroup _dialogueCanvasGroup;

    void Start() {
        _lineView = GetComponentInChildren<LineView>();
        if (_lineView == null)
            Debug.Log("can't find line view in " + name);
    }

    public void HideDialogueBox() {
        _dialogueCanvasGroup.alpha = 0;
    }

    public void ShowDialogueBox() {
        _dialogueCanvasGroup.alpha = 1;
    }
}
