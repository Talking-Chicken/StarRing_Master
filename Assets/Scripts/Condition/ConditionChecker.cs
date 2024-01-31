using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.InventoryEngine;

public class ConditionChecker : MonoBehaviour
{
    [SerializeField] public List<ConditionSet> setsOfConditions;
    public bool CheckCondition(int setNum)
    {
        return setsOfConditions[setNum].CheckCondition();
    }
    public bool CheckCondition(string name)
    {
        return setsOfConditions.Find(x => x.name == name).CheckCondition();
    }
    //check every set of conditions and return true when at least one set is eligible
    protected bool CheckConditionAll()
    {
        for (int i = 0; i < setsOfConditions.Count; i++)
        {
            if (CheckCondition(i))
            {
                return true;
            }
        }
        return false;
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
    [SerializeField] public string name;
    [SerializeField] public List<ComparisonCondition> conditions;

    public bool CheckCondition()
    {
        foreach (ComparisonCondition condition in conditions)
        {
            if (!condition.CheckCondition())
            {
                return false;
            }
        }
        return true;
    }
}