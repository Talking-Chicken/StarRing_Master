using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Yarn.Unity;

public class NPC : MonoBehaviour, ITalkable
{
    [SerializeField, BoxGroup("Dialogue")] private string startNodeBase;
    [ShowNonSerializedField, BoxGroup("Dialogue")] private Dictionary<string,int> interactCount = new Dictionary<string, int>();
    //getters & setters
    public string StartNodeBase {get=>startNodeBase;set=>startNodeBase=value;}
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.G))
            Debug.Log(interactCount["Helmet_1"]);
    }

    //ITalkable
    [YarnCommand("SetProgress")]
    public void SetProgress(string nodeBase) {
        if (interactCount.ContainsKey(nodeBase)) {
            interactCount[nodeBase]++;
        } else {
            interactCount.Add(nodeBase, 1);
        }
    }

    public int GetProgress(string nodeBase) {
        if (interactCount.ContainsKey(nodeBase))
            return interactCount[nodeBase];
        else
            return 0;
    }
}
