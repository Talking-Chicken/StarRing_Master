using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    [ReadOnly, SerializeField, BoxGroup("Info")] private Transform playerTransform;
    [ReadOnly, SerializeField, BoxGroup("Info")] private List<Button> options = new List<Button>();
    [ReadOnly, SerializeField, BoxGroup("Info")] private int currentOptionIndex = 0;
    [ReadOnly, SerializeField, BoxGroup("Info")] private float currentTime = 0;

    //getters & setters
    public int CurrentOptionIndex {get=>currentOptionIndex; set=>currentOptionIndex=value;}

    void Start()
    {
        //goes under player transform
        playerTransform = FindObjectOfType<PlayerManager>().gameObject.transform;
        transform.Rotate(new Vector3(0, 225, 0), Space.World);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) {
            transform.Rotate(new Vector3(0, 90, 0), Space.World);
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            transform.Rotate(new Vector3(0, -90, 0), Space.World);
        }
    }

    public void ChangeOption(int index) {
        if (CurrentOptionIndex == index) {
            Debug.Log("do smth");
        } else {
            // transform.Rotate(new Vector3(0, -90 * (CurrentOptionIndex - index), 0), Space.World);
            StartCoroutine(LerpToPoint(0.45f, -90.0f * (CurrentOptionIndex - index), transform.rotation.eulerAngles.y));
            CurrentOptionIndex = index;
        }
    }

    IEnumerator LerpToPoint(float lerpTime, float rotateValue, float angleY) {
        while (currentTime < lerpTime) {
            currentTime += Time.unscaledDeltaTime;
            float yValue = angleY + Mathf.Lerp(0.0f, rotateValue, currentTime/lerpTime);
            transform.rotation = Quaternion.Euler(transform.rotation.x, yValue, transform.rotation.z);
            yield return null;
        }
        Debug.Log("finished");
        currentTime = 0.0f;

    }
}
