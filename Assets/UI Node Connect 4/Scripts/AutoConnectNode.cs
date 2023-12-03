using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeadowGames.UINodeConnect4;

public class AutoConnectNode : MonoBehaviour
{
    public bool active;
    private List<ConnectedPorts> connectedPorts = new List<ConnectedPorts>();
    // Start is called before the first frame update
    void Start()
    {
        if (!active)
        {
            Node node = GetComponent<Node>();
            foreach (Port port in node.ports)
            {
                List<MeadowGames.UINodeConnect4.Connection> connectionsList = port.Connections;
                foreach (MeadowGames.UINodeConnect4.Connection conn in connectionsList)
                {
                    Port other = conn.port0 != port ? conn.port0 : conn.port1;
                    connectedPorts.Add(new ConnectedPorts(port, other));
                    Debug.Log(other.node.gameObject);
                    AutoConnectNode otherConnect = other.node.GetComponent<AutoConnectNode>();
                    if (otherConnect != null)
                    {
                        otherConnect.AddConnection(port, other);
                    }
                    conn.Remove();
                }
            }
            DisableSelf();
            //Invoke(nameof(DisableSelf), .01f);
        }
    }
    private void DisableSelf()
    {
        gameObject.SetActive(false);
        active = true;
    }
    public void AddConnection(Port port0, Port port1)
    {
        connectedPorts.Add(new ConnectedPorts(port0, port1));
    }
    private void OnEnable()
    {
        if (active)
        {
            foreach (ConnectedPorts connectedPort in connectedPorts)
            {
                connectedPort.Connecet();
            }
            Destroy(this);
        }

    }
    protected class ConnectedPorts
    {
        Port port0;
        Port port1;
        public ConnectedPorts(Port port0, Port port1)
        {
            this.port0 = port0;
            this.port1 = port1;
        }
        public void Connecet()
        {
            if (port0.gameObject.activeInHierarchy && port1.gameObject.activeInHierarchy)
                port0.ConnectTo(port1);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
