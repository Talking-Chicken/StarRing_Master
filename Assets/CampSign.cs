using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CampSign : InteractableObj
{
    [SerializeField] DialogueRunner dialogue;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Interact(PlayerProperty player)
    {
        base.Interact(player);
        dialogue.StartDialogue("ItemCampSign");
        StopInteract();
    }
}
