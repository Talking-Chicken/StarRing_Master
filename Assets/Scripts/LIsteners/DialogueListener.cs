using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using NaughtyAttributes;

/// <summary>
/// A Scriptable object listens all events that dealing with Dialogues. 
/// </summary>
[CreateAssetMenu(fileName = "Dialogue Listener", menuName = "Star Ring/Listeners/DialogueListener", order = 3)]
public class DialogueListener : ScriptableObject
{      
    [System.NonSerialized]
    public UnityEvent<DialogueUIManager> onCreatedEvent;
    [System.NonSerialized]
    public UnityEvent startNodeEvent, nodeCompleteEvent, startDialogueEvent, dialogueCompleteEvent, selectOptionEvent, onCommandEvent;

    [ReadOnly, SerializeField] private DialogueUIManager manager;
    [ReadOnly, SerializeField] private bool isCreated = false;

    //getters & setters
    public DialogueUIManager Manager {get=>manager; private set=>manager=value;}
    public bool IsCreated {get=>isCreated; set=>isCreated=value;}
    
    private void OnEnable() {
        if (onCreatedEvent == null)
            onCreatedEvent = new UnityEvent<DialogueUIManager>();
        if (startNodeEvent == null)
            startNodeEvent = new UnityEvent();
        if (nodeCompleteEvent == null)
            nodeCompleteEvent = new UnityEvent();
        if (startDialogueEvent == null)
            startDialogueEvent = new UnityEvent();
        if (dialogueCompleteEvent == null)
            dialogueCompleteEvent = new UnityEvent();
        if (selectOptionEvent == null)
            selectOptionEvent = new UnityEvent();
        if (onCommandEvent == null)
            onCommandEvent = new UnityEvent();
    }

    private void OnDisable() {
        IsCreated = false;
        Manager = null;
    }

    public DialogueUIManager OnCreated(DialogueUIManager manager) {
        if (manager != null) {
            IsCreated = true;
            Manager = manager;
            onCreatedEvent.Invoke(Manager);
            return manager;
        }
        return null;
    }

    public void StartNode() {startNodeEvent.Invoke();}

    public void NodeComplete() {nodeCompleteEvent.Invoke();}

    public void StartDialogue() {startDialogueEvent.Invoke();}

    public void DialogueComplete() {dialogueCompleteEvent.Invoke();}

    public void SelectOption() {selectOptionEvent.Invoke();}

    public void OnCommand() {onCommandEvent.Invoke();}
}
