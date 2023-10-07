using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ConditionSheetHandler
{
    const string filePath = "Assets/Resources/Conditions.tsv";
    private static string[,] tsvData;
    public static void ReadTSV()
    {
        string[] lines = File.ReadAllLines(filePath);

        int numRows = lines.Length;
        int numColumns = lines[0].Split('\t').Length;

        tsvData = new string[numRows, numColumns];

        for (int i = 0; i < numRows; i++)
        {
            string[] values = lines[i].Split('\t');
            for (int j = 0; j < numColumns; j++)
            {
                tsvData[i, j] = values[j];
            }
        }
    }
    public static List<Condition> CreateNewConditionArray()
    {
        if (tsvData == null)
        {
            ReadTSV();
        }
        if (tsvData == null)
        {
            Debug.LogError("TSV data not found");
            return null;
        }
        List<Condition> conditions = new List<Condition>();
        for (int i = 1; i < tsvData.GetLength(0); i++)
        {
            string name = tsvData[i, 0];
            string type = tsvData[i, 1];
            string value = tsvData[i, 2];
            if (type != "string" && value == "")
            {
                Debug.LogError($"Missing value in {name}, line {i}, type {type}");
            }
            else
            {
                switch (type)
                {
                    case "bool":
                        conditions.Add(new BoolCondition(name, bool.Parse(value.ToLower())));
                        break;
                    case "int":
                        conditions.Add(new IntCondition(name, int.Parse(value)));
                        break;
                    case "float":
                        conditions.Add(new FloatCondition(name, float.Parse(value)));
                        break;
                    case "string":
                        conditions.Add(new StringCondition(name, value));
                        break;
                    default:
                        Debug.LogError("Invalid condition type: " + type);
                        break;
                }
            }

        }
        return conditions;
    }
}
