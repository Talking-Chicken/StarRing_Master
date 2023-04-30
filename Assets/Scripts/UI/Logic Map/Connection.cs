using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*this is the connection line that connecting two information nodes*/
public class Connection : MonoBehaviour
{
    private Pin inputPin, outputPin;
    private LineRenderer line;
    private int currentDragingVertex;

    //getters & setters
    public Pin InputPin {get=>inputPin; set=>inputPin=value;}
    public Pin OutputPin {get=>outputPin; set=>outputPin=value;}
    public LineRenderer Line {get=>line; private set =>line=value;}

    //constructor
    public Connection(Pin pin, Pin.PinType pinType) {
        TryGetComponent<LineRenderer>(out line);

        if (Line == null)
            Debug.LogWarning("Can't find LineRenderer in " + name);

        switch (pinType) {
            case Pin.PinType.InputPin:
                InputPin = pin;
                Line.SetPosition(0, pin.connectingPoint.transform.position);
                break;
            case Pin.PinType.OutputPin:
                OutputPin = pin;
                Line.SetPosition(1, pin.connectingPoint.transform.position);
                break;
        }    
    }

    void Start() {
        
    }

    void Update() {
        
    }
}
