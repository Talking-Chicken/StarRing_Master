using MeadowGames.UINodeConnect4;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectNode : MonoBehaviour
{
    public Port otherPort;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        GetComponent<Port>().ConnectTo(otherPort);
        Destroy(this);
    }
}
