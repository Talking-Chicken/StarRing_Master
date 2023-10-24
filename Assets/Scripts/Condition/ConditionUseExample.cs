using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Yarn.Compiler.BasicBlock;

public class ConditionUseExample : MonoBehaviour
{
    public TMP_Text text;
    ConditionChecker condition;
    // Start is called before the first frame update
    void Start()
    {
        condition = GetComponent<ConditionChecker>();
        LoadConditionDavid();
    }

    public void LoadConditionDavid()
    {
        string alive;
        if (condition.CheckCondition("david"))
        {
            alive = "alive";
        }
        else
        {
            alive = "dead";
        }
        text.text = $"David is {alive}";
    }
    public void SetConditionDavid(bool isAlive)
    {
        ConditionSystemManager.SetBool("david_is_alive", isAlive);
        LoadConditionDavid();
    }
}
