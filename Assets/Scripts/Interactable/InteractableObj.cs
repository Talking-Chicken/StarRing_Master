using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj : Interactable
{
    [SerializeField] private Transform interactPosition;
    [SerializeField] private float delayStartInteract;

    //getters & setters
    public Transform InteractPosition {get=>interactPosition;}
    public float DelayStartInteract {get=>delayStartInteract;}

    protected override void Start()
    {
        base.Start();
        Type = InteractableType.OBJ;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Interact(PlayerProperty player)
    {
        base.Interact(player);
    }
}
