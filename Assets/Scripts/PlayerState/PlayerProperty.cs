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
    
    //getters & setters
    public GameObject Model {get=>model;}
}
