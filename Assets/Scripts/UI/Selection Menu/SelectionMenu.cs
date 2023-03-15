using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    [ReadOnly, SerializeField, BoxGroup("Info")] private Transform playerTransform;
    [ReadOnly, SerializeField, BoxGroup("Info")] private List<Button> options = new List<Button>();
    [ReadOnly, SerializeField, BoxGroup("Info")] private int currentOptionIndex = 1;
    [ReadOnly, SerializeField, BoxGroup("Info")] private float currentTime = 0;

    //getters & setters
    public int CurrentOptionIndex {get=>currentOptionIndex; set=>currentOptionIndex=value;}

    void Start()
    {
        //goes under player transform
        playerTransform = FindObjectOfType<PlayerManager>().gameObject.transform;
        transform.Rotate(new Vector3(0, 225, 0), Space.World);

        options.AddRange(GetComponentsInChildren<Button>());

        Debug.Log("start");
    }

    void Update()
    {
    }

    //used in options OnClick() functions
    //if the current selecting option is not the nearest to the camera one, rotate
    //otherwise change to the state that button is used for
    public void ChangeOption(int index) {
        if (CurrentOptionIndex == index) {
            Debug.Log("do smth");
        } else {
            // transform.Rotate(new Vector3(0, -90 * (CurrentOptionIndex - index), 0), Space.World);
            StartCoroutine(LerpToPoint(0.45f, -90.0f * (CurrentOptionIndex - index), transform.rotation.eulerAngles.y));
            CurrentOptionIndex = index;
            options[index].transform.SetAsLastSibling();
        }
    }

    //rotate to the point that the current selecting option is nearest to the camera
    IEnumerator LerpToPoint(float lerpTime, float rotateValue, float angleY) {
        while (currentTime < lerpTime) {
            currentTime += Time.unscaledDeltaTime;
            float yValue = angleY + Mathf.Lerp(0.0f, rotateValue, currentTime/lerpTime);
            transform.rotation = Quaternion.Euler(transform.rotation.x, yValue, transform.rotation.z);
            yield return null;
        }
        currentTime = 0.0f;
    }
}
