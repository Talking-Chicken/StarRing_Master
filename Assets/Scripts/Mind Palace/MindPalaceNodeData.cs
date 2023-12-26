using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
[JsonObject]
public class MindPalaceNodeData
{
    [JsonProperty] public float posX, posY;
    [JsonProperty] public string name;

    public MindPalaceNodeData(Vector2 position, string name)
    {
        posX = position.x;
        posY = position.y;
        this.name = name;
    }
}
/*[JsonObject]
public class MindPalacePort
{
    public enum Type { Input, Output }
    [JsonProperty] public Type portType;
    [JsonProperty] public int index;
    [JsonProperty] public string connectedToNodeName;
    [JsonProperty] public int connectedToPortIndex;
    public MindPalacePort(Type portType, int index, string connectedToNodeName, string connectedToPortIndex)
    {
        this.portType = portType;
        this.index = index;
        this.connectedToNodeName = connectedToNodeName;
        this.connectedToPortIndex = int.Parse(connectedToPortIndex);
    }
}*/
