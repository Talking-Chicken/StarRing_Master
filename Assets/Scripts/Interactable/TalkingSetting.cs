using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "Talking Setting", menuName = "Star Ring/Talking Setting", order = 1)]
public class TalkingSetting : ScriptableObject{
    public string StartNodeName;
    public CinemachineVirtualCamera TalkingCamera;
    public Vector3 TalkingPosition;
}
