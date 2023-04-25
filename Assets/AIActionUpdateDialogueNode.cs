using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class AIActionUpdateDialogueNode : AIAction
{
    // Start is called before the first frame update
    public string node_name;
    public NPC character;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void PerformAction()
    {
        character.StartNodeBase = node_name;
    }
}
