using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConditionUseExample : MonoBehaviour
{
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        LoadConditionDavid();
    }
    
    public void LoadConditionDavid()
    {
        string alive;
        if (ConditionSystemManager.GetBool("david_is_alive"))
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
