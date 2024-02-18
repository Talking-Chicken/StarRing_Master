using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Yarn.Unity;

public class NPC : Interactable, ITalkable
{
    [SerializeField, BoxGroup("Dialogue")] private string startNodeBase;
    [SerializeField, BoxGroup("Dialogue")] private string startNode;
    [ShowNonSerializedField, BoxGroup("Dialogue")] private Dictionary<string,int> interactCount = new Dictionary<string, int>();
    [SerializeField, BoxGroup("Dialogue")] private List<Transform> talkingPositions;
    [SerializeField, BoxGroup("Dialogue")] private List<TalkingSetting> talkingSettings;

    //getters & setters
    public string StartNodeBase {get=>startNodeBase;set=>startNodeBase=value;}
    public List<Transform> TalkingPositions {get=>talkingPositions; private set=>talkingPositions=value;}
    public List<TalkingSetting> TalkingSettings {get=>talkingSettings; set=>talkingSettings=value;}
    
    protected override void Start() {
        base.Start();
    }

    // protected override void Update() {
    //     base.Update();
    //     // if (Input.GetKeyDown(KeyCode.G))
    //     //     Debug.Log(interactCount["Helmet_1"]);

    //     currentState.UpdateState(this);
    // }

    public override void Interact(PlayerProperty player)
    {
        base.Interact(player);
        StartDialogue(startNode);
    }

    protected override void OnDialogueCompleted()
    {
        base.OnDialogueCompleted();
        StopInteract();
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
