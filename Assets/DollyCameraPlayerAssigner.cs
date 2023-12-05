using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class DollyCameraPlayerAssigner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<CinemachineVirtualCamera>().Follow==null)
        {
            GetComponent<CinemachineVirtualCamera>().Follow = FindAnyObjectByType<PlayerManager>().transform;
            GetComponent<CinemachineVirtualCamera>().LookAt = FindAnyObjectByType<PlayerManager>().transform;
        }
    }
}