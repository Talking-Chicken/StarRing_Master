using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : Investigation
{
    [SerializeField] private string shortnodeName;
    public override void Investigate(Interactable interactable)
    {
        base.Investigate(interactable);
        StartDialogue(shortnodeName);
    }
}
