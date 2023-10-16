using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionChecker : MonoBehaviour
{
    [SerializeField] public List<ConditionSet> setsOfConditions;
    public bool CheckCondition(int setNum)
    {
        return setsOfConditions[setNum].CheckCondition();
    }
}

[Serializable]
public class ComparisonCondition
{
    [SerializeField] string name;
    [SerializeField] Condition.Type type;
    [SerializeField] string value;
    public bool CheckCondition()
    {
        switch (type)
        {
            case Condition.Type.Bool:
                return ConditionSystemManager.GetBool(name) == bool.Parse(value);
            case Condition.Type.Float:
                return ConditionSystemManager.GetFloat(name) == float.Parse(value);
            case Condition.Type.Int:
                return ConditionSystemManager.GetInt(name) == int.Parse(value);
            case Condition.Type.String:
                return ConditionSystemManager.GetString(name) == value;
            default:
                Debug.LogError("Wrong Type");
                return false;
        }
    }
}
[Serializable]
public class ConditionSet
{
    [SerializeField] public List<ComparisonCondition> conditions;
    public bool CheckCondition()
    {
        foreach (ComparisonCondition condition in conditions)
        {
            if(!condition.CheckCondition())
            {
                return false;
            }
        }
        return true;
    }
}