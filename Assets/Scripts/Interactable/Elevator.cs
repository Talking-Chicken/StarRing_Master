using System.Collections;
using System.Collections.Generic;
using AK.Wwise;
using NaughtyAttributes;
using UnityEngine;

public class Elevator : Interactable
{
    [SerializeField, BoxGroup("Elevator")] private int level;
    [BoxGroup("Elevator")] private int targetLevel = 1;
    [BoxGroup("Elevator")] private List<Transform> objectsInElevator = new List<Transform>();
    [SerializeField, Foldout("Listeners")] private ElevatorActionListener _elevatorListener;


    //getters & setters
    public List<Transform> ObjectsInElevator {get=>objectsInElevator;}

    protected override void OnEnable()
    {
        base.OnEnable();
        _elevatorListener.teleport.AddListener(TeleportObjectsHere);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _elevatorListener.teleport.RemoveListener(TeleportObjectsHere);
    }

    public override void Interact(PlayerProperty player)
    {
        base.Interact(player);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            TriggerTeleport();
    }

    public void TriggerTeleport()
    {
        _elevatorListener.teleport.Invoke(this, targetLevel);
    }

    public void TeleportObjectsHere(Elevator startElevator, int level)
    {
        if (level != this.level)
            return;
        if (startElevator == this)
            return;

        Transform targetElevator = this.transform;
        if (targetElevator == null)
        {
            Debug.LogError("Target elevator not found for level: " + level);
            return;
        }

        // Calculate the position and rotation difference between the current and target elevators
        Vector3 positionOffset = targetElevator.position - startElevator.transform.position;
        Quaternion rotationOffset = Quaternion.Inverse(startElevator.transform.rotation) * targetElevator.rotation;

        foreach (Transform obj in startElevator.ObjectsInElevator)
        {
            // Calculate the new position and rotation for each object
            Vector3 newPosition = startElevator.transform.position + positionOffset + rotationOffset * (obj.position - startElevator.transform.position);
            Quaternion newRotation = rotationOffset * obj.rotation;

            // Teleport the object to the new position and rotation
            obj.position = newPosition;
            obj.rotation = newRotation;

            ObjectsInElevator.Add(obj);
        }
        startElevator.ObjectsInElevator.Clear();
        startElevator.StopInteract();
    }

    void OnTriggerEnter(Collider collider)
    {
        ObjectsInElevator.Add(collider.transform);
    }

    void OnTriggerExit(Collider collider)
    {
        ObjectsInElevator.Remove(collider.transform);
    }
}
