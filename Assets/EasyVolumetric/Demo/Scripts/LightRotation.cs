using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class used to rotate the light in the demo scene.
/// </summary>
public class LightRotation : MonoBehaviour
{

    [SerializeField]
    private Vector3 rotation;

    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, Space.World);
    }

}