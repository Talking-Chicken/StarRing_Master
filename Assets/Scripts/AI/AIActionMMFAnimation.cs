using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class AIActionMMFAnimation : AIAction
{
    // Start is called before the first frame update
    public MMF_Player animation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void PerformAction()
    {
        animation.PlayFeedbacks();
    }
}
