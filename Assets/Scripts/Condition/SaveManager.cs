using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public static class SaveManager
{
    const string fileName = "Star_Ring";
    private static SaveData saveData;

    public static List<Condition> GetConditions()
    {
        if (saveData == null)
        {
            LoadSaveFile();
        }
        return saveData.conditions;
    }
    public static void SaveConditions(List<Condition> conditions)
    {
        saveData.conditions = conditions;
        Debug.Log(conditions[0].name);
        SaveFile();
    }

    public static void SaveFile()
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonConvert.SerializeObject(saveData, Formatting.Indented);

        // Write JSON to file.
        File.WriteAllText(Application.persistentDataPath + $"/{fileName}.json", jsonString);
    }
    public static List<Condition> RecreateSaveFile()
    {
        return CreateNewSaveFile().conditions;
    }
    public static bool CheckSaveFileLength()
    {
        if (saveData == null || saveData.conditions == null)
        {
            return false;
        }
        else
        {
            return saveData.conditions.Count == ConditionSheetHandler.CreateNewConditionArray().Count;
        }
    }
    private static void LoadSaveFile()
    {
        string saveFilePath = Application.persistentDataPath + $"/{fileName}.json";
        if (File.Exists(saveFilePath))
        {
            //use custom converter
            JsonSerializerSettings settings = new();
            settings.Converters.Add(new ConditionConverter());

            string fileContents = File.ReadAllText(saveFilePath);
            saveData = JsonConvert.DeserializeObject<SaveData>(fileContents, settings);

            //check if the length of save file equals condition form
            if (saveData != null && !CheckSaveFileLength())
            {
                Debug.LogError($"condition file from save data({saveData.conditions.Count} columns) is not same as condition sheet ({ConditionSheetHandler.CreateNewConditionArray().Count} columns) ");
                saveData.conditions = RecreateSaveFile();
            }

        }
        //create new save file
        if (saveData == null)
        {
            CreateNewSaveFile();
        }
    }
    private static SaveData CreateNewSaveFile()
    {
        List<Condition> conditions = ConditionSheetHandler.CreateNewConditionArray();
        saveData = new SaveData(conditions);
        return saveData;
    }
}

public class SaveData
{
    [JsonProperty] public List<Condition> conditions;
    public SaveData(List<Condition> conditions)
    {
        this.conditions = conditions;
    }
}