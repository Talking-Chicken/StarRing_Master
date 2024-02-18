using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class FPSCameraControl : MonoBehaviour
{
    public float longPressDuration = 2.0f; // 长按持续时间
    public float mouseSensitivity = 100.0f;
    Transform playerBody;
    [SerializeField] float radiogaze=3;
    [SerializeField] Canvas canvas;
    [SerializeField] Image progress;
    [SerializeField] TextMeshProUGUI item_name;
    [SerializeField] CinemachineVirtualCamera camera;
    private bool isPressing = false; // 是否正在长按
    private float pressTimer = 0f; // 记录长按时间
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private RaycastHit hit;
    public bool Activied=false; 
    void Start()
    {
       // 
        playerBody = this.transform;
    }

    void Update()
    {
        if (Activied) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (Input.GetMouseButtonDown(1)) 
            {
                Cursor.lockState= CursorLockMode.None;

                Activied = false;
            }
        }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;
        // xRotation = Mathf.Clamp(xRotation, -90f, 90f); 
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
      //  canvas.transform.position = hit.collider.GetComponent<Investigation>().ui_location.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Investigation")
            {
                canvas.transform.position = hit.collider.GetComponent<Investigation>().ui_location.position;
                canvas.transform.LookAt(camera.transform.position, Vector3.up);
                item_name.text = hit.collider.GetComponent<Investigation>().interact_name;
                //   Debug.Log(hit.collider.GetComponent<Investigation>().interact_name);
                if (Input.GetMouseButtonDown(0))
                {
                    isPressing = true; // 标记正在长按
                    pressTimer = 0f; // 重置长按计时器
                }
                // 如果鼠标抬起
                else if (Input.GetMouseButtonUp(0))
                {
                    isPressing = false; // 标记长按结束
                    pressTimer = 0;
                    if (pressTimer < longPressDuration)
                    {
                        hit.collider.GetComponent<Investigation>().InvestigationContent();
                    }
                    else
                    {
                        // 长按逻辑
                        Debug.Log("Long Press");
                    }
                }

                // 如果正在长按
                if (isPressing)
                {

                    pressTimer += Time.deltaTime;
                    
                    if (pressTimer >= longPressDuration)
                    {
                        // 长按逻辑
                        Debug.Log("Long Press");
                        // 这里可以添加你想要执行的长按逻辑
                    }
                }
                progress.fillAmount = pressTimer / longPressDuration;
            }

        }
        else
        {
            progress.fillAmount = 0;
            item_name.text = "";
            

        }
    }
}
