using MeadowGames.UINodeConnect4;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MindPalaceManager : MonoBehaviour
{
    List<Node> nodes = new();
    public static MindPalaceManager activeManager;
    private void Awake()
    {
        activeManager = this;
        nodes = GetComponentsInChildren<Node>().ToList();
    }
    // Start is called before the first frame update
    void Start()
    {
        MindPalaceDataSystem.LoadMindPalace(nodes);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveNodes();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            InstantiateSaveFile();
        }
    }
    public void SaveNodes()
    {
        List<Node> activeNodes = nodes.Where(node => node.gameObject.activeSelf).ToList();
        MindPalaceDataSystem.SaveMindPalace(activeNodes);
    }
    public List<Node> InstantiateSaveFile()
    {
        List<Node> activeNodes = nodes.Where(node => node.GetComponent<MindPalaceNode>().active).ToList();
        MindPalaceDataSystem.SaveMindPalace(activeNodes);
        return activeNodes;
    }
    public void ActiveNode(string name)
    {
        Node node = nodes.Find(node => node.ID.Equals(name,System.StringComparison.OrdinalIgnoreCase));
        if (node != null)
        {
            node.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Node not found");
        }
    }
    public bool GetNodeActive(string name)
    {
        return nodes.Find(node => node.ID.Equals(name, System.StringComparison.OrdinalIgnoreCase)).gameObject.activeSelf;
    }
    public void SetNodeState(string name, int state)
    {
        nodes.Find(node => node.ID.Equals(name, System.StringComparison.OrdinalIgnoreCase)).GetComponent<MindPalaceNode>().SetState(state);
    }
    public int GetNodeState()
    {
        return nodes.Find(node => node.ID.Equals(name, System.StringComparison.OrdinalIgnoreCase)).GetComponent<MindPalaceNode>().State;
    }
}
