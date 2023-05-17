using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}
