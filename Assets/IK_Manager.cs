using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Manager : MonoBehaviour
{
    Animator animator;
    PlayerManager Player;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Player = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnAnimatorIK(GameObject Target)
    {
      // if (animator.GetCurrentAnimatorStateInfo(0).IsName("ready_look"))
      
        {

            animator.SetLookAtWeight(0.5f);
            animator.SetLookAtPosition(Target.transform.position + new Vector3(0, 1.5f, 0));
        }
    }
}
