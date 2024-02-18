using MeadowGames.UINodeConnect4;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MindPalaceDataSystem
{
    // Start is called before the first frame update
    public static void LoadMindPalace(List<Node> nodes)
    {
        List<MindPalaceNodeData> nodeData = SaveManager.GetMindPalaceNodes();
        if (nodeData == null || nodeData.Count <= 0)
        {
            MindPalaceManager.activeManager.InstantiateSaveFile();
            nodeData = SaveManager.GetMindPalaceNodes();
        }
        foreach (Node node in nodes)
        {
            bool found = false;
            //id of node can be find in nodeData
            foreach (MindPalaceNodeData nodeSave in nodeData)
            {
                if (nodeSave.name == node.ID)
                {
                    found = true;
                    node.GetComponent<RectTransform>().anchoredPosition = new Vector2(nodeSave.posX, nodeSave.posY);
                    node.GetComponent<MindPalaceNode>().SetState(nodeSave.state);
                    break;
                }
            }
            if (!found)
            {
                node.GetComponent<MindPalaceNode>().DisableSelf();
            }
        }
    }
    public static void SaveMindPalace(List<Node> nodes)
    {
        List<MindPalaceNodeData> nodeData = new();
        foreach (Node node in nodes)
        {
            nodeData.Add(new MindPalaceNodeData(node.GetComponent<RectTransform>().anchoredPosition, node.ID, node.GetComponent<MindPalaceNode>().State));
        }
        SaveManager.SaveMindPalace(nodeData);
    }
}
