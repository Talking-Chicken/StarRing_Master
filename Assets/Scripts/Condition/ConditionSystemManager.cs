using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ConditionSystemManager
{
    private static List<Condition> conditions;
    public static void LoadConditions()
    {
        conditions = SaveManager.GetConditions();
    }
    public static void SaveConditions()
    {
        SaveManager.SaveConditions(conditions);
    }
    public static bool GetBool(string name)
    {
        if (conditions == null)
        {
            LoadConditions();
        }
        Condition condition = conditions.Where(x => x.name == name).FirstOrDefault();
        if (condition == null)
        {
            Debug.Log($"Condition {name} does not exist");
            return false;
        }
        else if (condition.GetType() == typeof(BoolCondition))
        {
            return ((BoolCondition)condition).GetValue();
        }
        else
        {
            Debug.LogError($"{name} is not a bool");
            return false;
        }
    }
    public static void SetBool(string name, bool value)
    {
        if (conditions == null)
        {
            LoadConditions();
        }
        Condition condition = conditions.Where(x => x.name == name).FirstOrDefault();
        if (condition == null)
        {
            Debug.Log($"Condition {name} does not exist");
        }
        else if (condition.GetType() == typeof(BoolCondition))
        {
            ((BoolCondition)condition).SetValue(value);
            SaveManager.SaveConditions(conditions);
        }
        else
        {
            Debug.Log(condition.GetType());
            Debug.LogError($"{name} is not a bool");
        }
    }
    public static int GetInt(string name)
    {
        if (conditions == null)
        {
            LoadConditions();
        }
        Condition condition = conditions.Where(x => x.name == name).FirstOrDefault();
        if (condition == null)
        {
            Debug.Log($"Condition {name} does not exist");
            return 0;
        }
        else if (condition.GetType() == typeof(IntCondition))
        {
            return ((IntCondition)condition).GetValue();
        }
        else
        {
            Debug.LogError($"{name} is not a int");
            return 0;
        }
    }
    public static void SetInt(string name, int value)
    {
        if (conditions == null)
        {
            LoadConditions();
        }
        Condition condition = conditions.Where(x => x.name == name).FirstOrDefault();

        if (condition == null)
        {
            Debug.Log($"Condition {name} does not exist");
        }
        else if (condition.GetType() == typeof(IntCondition))
        {
            ((IntCondition)condition).SetValue(value);
            SaveManager.SaveConditions(conditions);
        }
        else
        {
            Debug.LogError($"{name} is not a int");
        }
    }
    public static float GetFloat(string name)
    {
        if (conditions == null)
        {
            LoadConditions();
        }
        Condition condition = conditions.Where(x => x.name == name).FirstOrDefault();
        if (condition == null)
        {
            Debug.Log($"Condition {name} does not exist");
            return 0;
        }
        else if (condition.GetType() == typeof(FloatCondition))
        {
            return ((FloatCondition)condition).GetValue();
        }
        else
        {
            Debug.LogError($"{name} is not a float");
            return 0;
        }
    }
    public static void SetFloat(string name, float value)
    {
        if (conditions == null)
        {
            LoadConditions();
        }
        Condition condition = conditions.Where(x => x.name == name).FirstOrDefault();
        if (condition == null)
        {
            Debug.Log($"Condition {name} does not exist");
        }
        else if (condition.GetType() == typeof(FloatCondition))
        {
            ((FloatCondition)condition).SetValue(value);
            SaveManager.SaveConditions(conditions);
        }
        else
        {
            Debug.LogError($"{name} is not a float");
        }
    }
    public static string GetString(string name)
    {
        if (conditions == null)
        {
            LoadConditions();
        }
        Condition condition = conditions.Where(x => x.name == name).FirstOrDefault();
        if (condition == null)
        {
            Debug.Log($"Condition {name} does not exist");
            return null;
        }
        else if (condition.GetType() == typeof(StringCondition))
        {
            return ((StringCondition)condition).GetValue();
        }
        else
        {
            Debug.LogError($"{name} is not a string");
            return "";
        }
    }
    public static void SetString(string name, string value)
    {
        if (conditions == null)
        {
            LoadConditions();
        }
        Condition condition = conditions.Where(x => x.name == name).FirstOrDefault();
        if (condition == null)
        {
            Debug.Log($"Condition {name} does not exist");
        }
        else if (condition.GetType() == typeof(StringCondition))
        {
            ((StringCondition)condition).SetValue(value);
            SaveManager.SaveConditions(conditions);
        }
        else
        {
            Debug.LogError($"{name} is not a string");
        }
    }

}
