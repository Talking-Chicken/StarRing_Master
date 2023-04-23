using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class AIActionLookAt : AIAction
{
    // Start is called before the first frame update
    public GameObject head;
    public Animator animator;
     PlayerManager Player;
    void Start()
    {
        Player = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void PerformAction()
    {

        Debug.Log("Player is:"+Player);
        Vector2 targetAngle = new Vector2(Player.transform.position.x - gameObject.transform.position.x, Player.transform.position.z - transform.position.z).normalized;
        //OnAnimatorIK();
      //  if (targetAngle.x<1&& targetAngle.x>-1)
      //  head.transform.LookAt(Player.gameObject.transform);

    }
    private void OnAnimatorIK()
    {
        if (animator)
        {
            animator.SetLookAtWeight(0.5f);
            animator.SetLookAtPosition(Player.transform.position);
        }
    }
}
