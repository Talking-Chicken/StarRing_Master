using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class switches demo scenes one by one by clicking any key.
/// </summary>
public class SceneSwitcher : MonoBehaviour
{

    private static SceneSwitcher instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        if (Input.anyKeyDown)
            NextScene();
    }

    private void NextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        if (++index >= SceneManager.sceneCountInBuildSettings)
            index = 0;

        Input.ResetInputAxes();
        SceneManager.LoadScene(index);
    }

}
