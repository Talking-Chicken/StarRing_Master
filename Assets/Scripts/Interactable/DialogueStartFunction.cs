using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class DialogueStartFunction : MonoBehaviour
{
    public GameObject PhoneChat;
    public DialogueRunner dialogueRunner;
    // 由UI按钮调用的方法
    public void newNode()
    {
        PhoneChat.SetActive(true);
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue("NewInformation");
    }
    public void newQuestion()
    {
        PhoneChat.SetActive(true);
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue("NewProblem");
    }
}
