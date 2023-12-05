using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class AreaCameraFeedbackPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MMF_Player areaCamFeedback;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            areaCamFeedback.PlayFeedbacks();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            areaCamFeedback.RestoreInitialValues();
        }
    }

}
