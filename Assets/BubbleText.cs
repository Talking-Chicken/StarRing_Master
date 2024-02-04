using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class BubbleText : MonoBehaviour
{
    // Start is called before the first frame update
    
    PlayerManager Amo;
    public bool idle;
    [SerializeField] GameObject bubbleText;
    private bool once = true;
    DialogueRunner bubbleRunner;
    [SerializeField] DialogueRunner
    [SerializeField] string nodeName;
    GameObject spawnedObject;
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
   
        if (idle)
        {
            if (Amo == null)
            {
                Amo = FindObjectOfType<PlayerManager>();
            }

            if (Vector3.Distance(transform.position, Amo.transform.position) < 1&&once)
            {
                once = false;
                spawnedObject = Instantiate(bubbleText);
                bubbleRunner = spawnedObject.GetComponentInChildren<DialogueRunner>();
            
                bubbleRunner.StartDialogue(nodeName);
               
            }

            if (Vector3.Distance(transform.position, Amo.transform.position) >3)
            {
                once = true;
                Destroy(spawnedObject);
            }
        }
    }
}
