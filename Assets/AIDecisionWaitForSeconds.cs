using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class AIDecisionWaitForSeconds : AIDecision
{
    // Start is called before the first frame update
    public float timer;
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
       
    }
    public override bool Decide()
    {
        return CheckTime();
    }
    bool CheckTime()
    {
        return (timer <= 0);
     
    }
}
