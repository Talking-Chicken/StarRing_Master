using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// property of player
/// </summary>
public class PlayerProperty : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private float rotateSpeed;
    
    //getters & setters
    public GameObject Model {get=>model;}
    public float RotateSpeed {get=>rotateSpeed;}
}
