using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionCheckerSpawner : ConditionChecker
{
    private void Awake()
    {
        SetActiveToCondition();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetActiveToCondition()
    {
        gameObject.SetActive(CheckConditionAll());
    }
}
