using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue Action Listener", menuName = "Star Ring/Listeners/Dialogue Action Listener", order = 1)]
public class DialogueActionListener : ScriptableObject
{
    /// <summary>
    /// param: string start node name
    /// </summary>
    [System.NonSerialized] 
    public UnityEvent<string> startDialogue;
    [System.NonSerialized]
    public UnityEvent stopDialogue;
    [System.NonSerialized]
    public UnityEvent nextLine;
    /// <summary>
    /// when dialogue has completed, this will be invoked
    /// </summary>
    [System.NonSerialized]
    public UnityEvent dialogueCompleted;

    void OnEnable() {
        if (startDialogue == null)
            startDialogue = new UnityEvent<string>();
        if (stopDialogue == null)
            stopDialogue = new UnityEvent();
        if (nextLine == null)
            nextLine = new UnityEvent();
        if (dialogueCompleted == null)
            dialogueCompleted = new UnityEvent();
    }
}
