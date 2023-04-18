using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMenuCamera : MonoBehaviour
{
    private Camera _mainCamera, myCam;
    void Start()
    {
        _mainCamera = Camera.main;
        myCam = GetComponent<Camera>();
    }

    
    void Update()
    {
        transform.position = _mainCamera.transform.position;
        transform.rotation = _mainCamera.transform.rotation;
        myCam.fieldOfView = _mainCamera.fieldOfView;
    }
}
