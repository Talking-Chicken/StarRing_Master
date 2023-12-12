using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor.Search;
public class ElevatorController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera mainCamera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space) )
        {
            DoorClose();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            DoorOpen();
        }
    }
    private void DoorClose()
    {

        int wallLayerIndex = LayerMask.NameToLayer("FloorAndWall");

        mainCamera.cullingMask &= ~(1 << wallLayerIndex);
    }
    void DoorOpen()
    {
        int wallLayerIndex = LayerMask.NameToLayer("FloorAndWall");
        mainCamera.cullingMask |= (1 << wallLayerIndex); 
    }
}
