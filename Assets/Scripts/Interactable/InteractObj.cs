using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class InteractObj : Interactable
{
    private PlayerManager player;
    [SerializeField, BoxGroup("Properties")] private bool isChangingToInteractState;
    [SerializeField, BoxGroup("Properties")] private Transform interactPosition;
    [SerializeField, BoxGroup("Properties")] private float delayStartInteract;

    //getters & setters
    public Transform InteractPosition {get=>interactPosition;}
    protected PlayerManager Player {get=>player;}
    public float DelayStartInteract {get=>delayStartInteract;}

    protected override void Start()
    {
        StartCoroutine(FindReference());
        base.Start();
    }

    public virtual bool Interact() {
        if (isChangingToInteractState) {
            Player.ChangeState(Player.stateInteract);
        }
        return true;
    }

    IEnumerator FindReference() {
        yield return new WaitForSeconds(0.1f);
        player = FindObjectOfType<PlayerManager>();
    }
}
