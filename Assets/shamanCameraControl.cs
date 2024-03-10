using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class shamanCameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public float longPressDuration = 2.0f; // 长按持续时间
    public float mouseSensitivity = 20.0f;
    Transform playerBody;
  
 
  
  
    private float pressTimer = 0f; // 记录长按时间
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private RaycastHit hit;
    public bool Activied = false;
    public bool canControl = true;
    public static bool isLongPressed = false;
    public float smoothTime = 0.1f;
    public float driftAmount;
    public float driftSensitivity;
    public DialogueRunner dialogue;
    Vector3 screenCenter;
    void Start()
    {
         screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {

        //   Cursor.lockState = CursorLockMode.Locked;


        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 总是应用飘逸效果，无论是否有鼠标输入
        float driftX = Random.Range(-driftAmount, driftAmount) * driftSensitivity * Time.deltaTime;
        float driftY = Random.Range(-driftAmount, driftAmount) * driftSensitivity * Time.deltaTime;

        // 将飘逸效果应用到旋转逻辑上
        xRotation -= (mouseY + driftY);
        yRotation += (mouseX + driftX);

        xRotation = Mathf.Clamp(xRotation, -20f, 20f); // 保持这个限制以避免翻转
        yRotation = Mathf.Clamp(yRotation, 80f, 100f);
        Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        // 使用Quaternion.Slerp插值到目标旋转
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothTime);

        // 玩家身体的水平旋转也许需要平滑处理，这取决于你的需求
        //playerBody.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        //  canvas.transform.position = hit.collider.GetComponent<Investigation>().ui_location.position;
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Mask")
            {
                pressTimer = 0;
                Debug.Log("see");
            }
            else 
            {
                pressTimer += Time.deltaTime;
                if (pressTimer >= longPressDuration)
                {
                    pressTimer = 0;
                    dialogue.Stop();
                    dialogue.StartDialogue("Amodaydream");                    Debug.Log("not see");
                }
            }
        }


    }
}
