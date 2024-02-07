using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coffee_Machine : Interactable
{
    protected override void Start()
    {
        base.Start();
    }
    public override void Interact(PlayerProperty player)
    {
        base.Interact(player);
        // _dialogueListener.startDialogue.Invoke("Argument1");
        // SceneManager.LoadScene("Level-Night");
        StartDialogue("coffeebreak");
        //  StopInteract();
    }
}
