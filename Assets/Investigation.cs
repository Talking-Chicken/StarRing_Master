using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Investigation : Interactable
{
    // Start is called before the first frame update
    public string interact_name;
    public Transform ui_location;
 
    /*[SerializeField] GameObject bubbleText;
    private bool once = true;
    DialogueRunner bubbleRunner;*/
    [SerializeField] string shortnodeName;
    [SerializeField] string longnodeName;
    PlayerProperty player;
    // GameObject spawnedObject;

    private void Start()
    {
        player=FindObjectOfType<PlayerProperty>();
    }

    public void InvestigationContent() 
    {
      //  base.Interact(player);
        StartDialogue(shortnodeName);
      //  ChangeState(stateDialogue);
    }
   
    public void ActualInvestigationContent()
    {
        StartDialogue(longnodeName);
    }
}
