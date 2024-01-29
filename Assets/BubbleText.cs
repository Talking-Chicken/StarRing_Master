using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class BubbleText : MonoBehaviour
{
    // Start is called before the first frame update
    
    PlayerManager Amo;
    public bool idle;
    bool once = true;
    [SerializeField] DialogueRunner bubbleRunner;
    [SerializeField] string nodeName;
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
                bubbleRunner.StartDialogue(nodeName);
                once = false;
            }

            if (Vector3.Distance(transform.position, Amo.transform.position) >3)
            {
                once = true;
            }
        }
    }
}
