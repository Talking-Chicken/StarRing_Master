using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class DialogueStartFunction : MonoBehaviour
{
    public GameObject PhoneChat;
    public DialogueRunner dialogueRunner;
    // ��UI��ť���õķ���
    public void openEvent()
    {
        PhoneChat.SetActive(true);
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue("Start");
    }
}
