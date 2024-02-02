using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCameraControl : MonoBehaviour
{

    public float mouseSensitivity = 100.0f;
     Transform playerBody;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    void Start()
    {
      //  Cursor.lockState = CursorLockMode.Locked; 
        playerBody = this.transform;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;
        // xRotation = Mathf.Clamp(xRotation, -90f, 90f); 
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Select stage    
                if (hit.transform.name == "lamp")
                {
                    Debug.Log("Test");
                }
            }
        }
    }
}
