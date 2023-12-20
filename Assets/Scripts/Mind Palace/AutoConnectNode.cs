using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeadowGames.UINodeConnect4;
using static MoreMountains.Tools.MMGizmo;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class AutoConnectNode : MonoBehaviour, IPointerDownHandler
{
    public bool active;
    private List<ConnectedPorts> connectedPorts = new List<ConnectedPorts>();
    private List<RelatedNode> relatedNodes = new();

    public bool onDragging = false;

    private RectTransform rectTransform;
    //range of related node
    const float relatedRange = 800f;
    const float reactiveTime = 2.5f;

    public bool Reacting { get; private set; } = false;
    // Start is called before the first frame update
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        //record other node
        Node node = GetComponent<Node>();
        foreach (Port port in node.ports)
        {
            List<MeadowGames.UINodeConnect4.Connection> connectionsList = port.Connections;
            foreach (MeadowGames.UINodeConnect4.Connection conn in connectionsList)
            {
                Port other = conn.port0 != port ? conn.port0 : conn.port1;
                if (other.Polarity == Port.PolarityType._in)
                {
                    if (other.Connections.Count == 2)
                    {
                        MeadowGames.UINodeConnect4.Connection otherConnection = other.Connections.Find(connection => connection.port0 != port && connection.port1 != port);
                        Node relatedNode = otherConnection.port0 != other ? otherConnection.port0.node : otherConnection.port1.node;
                        relatedNodes.Add(new(relatedNode, other.node));
                    }
                }
            }
        }
    }
    void Start()
    {
        /*if (!active)
        {
            DisableSelf();
        }*/
    }
    private void Update()
    {
        if (onDragging && Input.GetMouseButtonUp(0))
        {
            onDragging = false;
        }
        if (onDragging && !Reacting)
        {
            //check if related node is in range
            foreach (RelatedNode related in relatedNodes.Where(node => node.GetRelatedActive()))
            {
                Debug.Log(related.GetRelatedNode().ID + "," + related.GetRelatedDistance(rectTransform.anchoredPosition));
                if (related.GetRelatedDistance(rectTransform.anchoredPosition) < relatedRange)
                {
                    //make other Node react
                    StartCoroutine(ReactToRelatedNode(related.GetRelatedNode().GetComponent<AutoConnectNode>(), related.GetRevealNode()));
                }
            }
        }
    }
    private IEnumerator ReactToRelatedNode(AutoConnectNode node, Node nextNode)
    {
        Reacting = true;
        float timer = 0;
        while (node && onDragging && Vector2.Distance(node.rectTransform.anchoredPosition, rectTransform.anchoredPosition) < relatedRange)
        {
            GetComponent<Image>().color = node.GetComponent<Image>().color = Color.Lerp(Color.white, Color.green, timer / reactiveTime);
            if (timer > reactiveTime)
            {
                //react
                nextNode.gameObject.SetActive(true);
                Debug.Log("react");
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        GetComponent<Image>().color = node.GetComponent<Image>().color = Color.white;
        Reacting = false;
    }
    public void DisableSelf()
    {
        Node node = GetComponent<Node>();
        foreach (Port port in node.ports)
        {
            List<MeadowGames.UINodeConnect4.Connection> connectionsList = port.Connections;
            foreach (MeadowGames.UINodeConnect4.Connection conn in connectionsList)
            {
                Port other = conn.port0 != port ? conn.port0 : conn.port1;
                connectedPorts.Add(new ConnectedPorts(port, other));
                //Debug.Log(other.node.gameObject);
                AutoConnectNode otherConnect = other.node.GetComponent<AutoConnectNode>();
                if (otherConnect != null)
                {
                    otherConnect.AddConnection(port, other);
                }
                conn.Remove();
            }
        }
        gameObject.SetActive(false);
        active = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        onDragging = true;
        Debug.Log(this.gameObject.name + " Was Clicked.");
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
        }

    }
    protected class ConnectedPorts
    {
        Port selfPort;
        Port otherPort;
        public ConnectedPorts(Port port0, Port port1)
        {
            this.selfPort = port0;
            this.otherPort = port1;
        }
        public void Connecet()
        {
            if (selfPort.gameObject.activeInHierarchy && otherPort.gameObject.activeInHierarchy)
                selfPort.ConnectTo(otherPort);
        }
    }
    protected class RelatedNode
    {
        protected Node relatedNode;
        protected Node revealNode;
        public RelatedNode(Node relatedNode, Node revealNode)
        {
            this.relatedNode = relatedNode;
            this.revealNode = revealNode;
        }
        public bool GetRelatedActive()
        {
            return relatedNode.gameObject.activeSelf && !revealNode.gameObject.activeSelf;
        }
        public float GetRelatedDistance(Vector2 pos)
        {
            return (relatedNode.GetComponent<RectTransform>().anchoredPosition - pos).magnitude;
        }
        public Node GetRevealNode()
        {
            return revealNode;
        }
        public Node GetRelatedNode()
        {
            return relatedNode;
        }
    }
}
