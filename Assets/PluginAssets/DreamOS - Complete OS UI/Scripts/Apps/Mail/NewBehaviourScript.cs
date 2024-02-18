using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.DreamOS; // DreamOS namespace

public class SampleClass : MonoBehaviour
{
    [SerializeField] private MailManager mailApp;
    [SerializeField] private MailItem mailItemToBeCreated;

    void AppFunctions()
    {
        // Add a new command
        mailApp.fromPrefix = "<"; // Change from (mail) prefix
        mailApp.fromSuffix = ">"; // Change from (mail) suffix
        mailApp.InitializeMails(); // Initialize all mail items
    }

    void CreateMailAtRuntime()
    {
        MailManager.MailAsset item = new MailManager.MailAsset();
        item.mailAsset = mailItemToBeCreated;
        mailApp.mailList.Add(item);
        mailApp.InitializeMails();
    }
}