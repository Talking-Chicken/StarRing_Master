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
    [System.NonSerialized] public UnityEvent<string> startDialogue;

    void OnEnable() {
        if (startDialogue == null)
            startDialogue = new UnityEvent<string>();
    }
}
