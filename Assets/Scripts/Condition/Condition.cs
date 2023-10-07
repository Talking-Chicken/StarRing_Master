using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

[JsonObject]
public class Condition
{
    [JsonProperty] public readonly string name;
    [JsonProperty] public readonly string type;
    public Condition(string name, string type)
    {
        this.name = name;
        this.type = type;
    }
    public bool CheckName(string name)
    {
        return this.name == name;
    }
    public bool CheckNameIgnoreCase(string name)
    {
        return name.Equals(this.name, System.StringComparison.CurrentCultureIgnoreCase);
    }
}
[JsonObject]
public class BoolCondition : Condition
{
    [JsonProperty] private bool value;
    public BoolCondition(string name, bool value) : base(name, "bool")
    {
        this.value = value;
    }
    public bool GetValue()
    {
        return value;
    }
    public void SetValue(bool value)
    {
        this.value = value;
    }
}
[JsonObject]
public class IntCondition : Condition
{
    [JsonProperty] private int value;
    public IntCondition(string name, int value) : base(name, "int")
    {
        this.value = value;
    }
    public int GetValue()
    {
        return value;
    }
    public void SetValue(int value)
    {
        this.value = value;
    }
}
[JsonObject]
public class FloatCondition : Condition
{
    [JsonProperty] private float value;
    public FloatCondition(string name, float value) : base(name, "float")
    {
        this.value = value;
    }
    public float GetValue()
    {
        return value;
    }
    public void SetValue(float value)
    {
        this.value = value;
    }
}
[JsonObject]
public class StringCondition : Condition
{
    [JsonProperty] private string value;
    public StringCondition(string name, string value) : base(name, "string")
    {
        this.value = value;
    }
    public string GetValue()
    {
        return value;
    }
    public void SetValue(string value)
    {
        this.value = value;
    }
}

//for json file Deserialize
public class ConditionConverter : JsonConverter<Condition>
{
    public override Condition ReadJson(JsonReader reader, Type objectType, Condition existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);
        string name = jsonObject.GetValue("name").ToString();
        string type = jsonObject.GetValue("type").ToString();


        /*JObject jsonObject = JObject.Load(reader);
        return serializer.Deserialize<IntCondition>(jsonObject.CreateReader());*/

        if (type == "bool")
        {
            return new BoolCondition(name, bool.Parse(jsonObject.GetValue("value").ToString()));
        }
        else if (type == "string")
        {
            return new StringCondition(name, jsonObject.GetValue("value").ToString());
        }
        else if (type == "float")
        {
            return new FloatCondition(name, float.Parse(jsonObject.GetValue("value").ToString()));
        }
        else if (type == "int")
        {
            return new IntCondition(name, int.Parse(jsonObject.GetValue("value").ToString()));
        }
        else
        {
            Debug.LogError("Type not found");
            return null;
        }
    }

    public override void WriteJson(JsonWriter writer, Condition value, JsonSerializer serializer)
    {
        // Implement serialization if needed

    }
}