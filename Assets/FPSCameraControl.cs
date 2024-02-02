using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCameraControl : MonoBehaviour
{

    public float mouseSensitivity = 100.0f;
     Transform playerBody; // 用于旋转玩家身体，以便摄像机与玩家同向

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 锁定光标到屏幕中心
        playerBody = this.transform;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;
       // xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 限制上下旋转角度，防止翻转
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
