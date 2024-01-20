using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Manager : MonoBehaviour
{
    Animator animator;
    PlayerManager Player;
    public GameObject target;
    public bool lookat;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lookat = false;
    }

    public void OnAnimatorIK()
    {
        // if (animator.GetCurrentAnimatorStateInfo(0).IsName("ready_look"))

        if (lookat)
        {
            animator.SetLookAtWeight(0.5f);
            animator.SetLookAtPosition(target.transform.position + new Vector3(0, 1.5f, 0));
        }


    }
}
