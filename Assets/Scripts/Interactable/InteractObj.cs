using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class InteractObj : Interactable
{
    [SerializeField, BoxGroup("Properties")] private Transform interactPosition;

    //getters & setters
    public Transform InteractPosition {get=>interactPosition;}

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public virtual bool Interact() {
        return true;
    }
}
