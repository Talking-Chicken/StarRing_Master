using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SelectionMenu : MonoBehaviour
{
    [SerializeField, BoxGroup("Info")] private Transform playerTransform;
    void Start()
    {
        //looking at the camera
        transform.LookAt(Camera.main.transform.position, transform.up);
    }

    void Update()
    {
        
    }
}
