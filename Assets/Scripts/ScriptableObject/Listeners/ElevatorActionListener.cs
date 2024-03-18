using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Elevator Action Listener", menuName = "Star Ring/Listeners/Elevator Action Listener", order = 1)]
public class ElevatorActionListener : ScriptableObject
{
    [System.NonSerialized]
    public UnityEvent<Elevator, int> getElevatorByLevel; //param: Elevator the elevator that invoke the event, int elevator level;
    [System.NonSerialized]
    public UnityEvent<Elevator, int> teleport; //param: Elevator the elevator that invoke the event, int elevator level;

    void OnEnable()
    {
        if (getElevatorByLevel == null)
            getElevatorByLevel = new UnityEvent<Elevator, int>();
        if (teleport == null)
            teleport = new UnityEvent<Elevator, int>();
    }
}
