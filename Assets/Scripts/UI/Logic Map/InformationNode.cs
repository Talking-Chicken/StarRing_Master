using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;

/*this is class for pin, which is in charge of connecting lines of Connection for different InformationNode*/
public class Pin {
    public enum PinType {InputPin, OutputPin}
    public GameObject connectingPoint;
}

/*this is the UI representation of information*/
public class InformationNode : MonoBehaviour
{
    private Information info;
    private List<Pin> inputList, outputList;

    [SerializeField, BoxGroup("UI")] private TextMeshProUGUI nodeName, shortDes, longDes;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
