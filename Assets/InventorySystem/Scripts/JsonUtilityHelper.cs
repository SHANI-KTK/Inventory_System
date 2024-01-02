using System.IO;
using UnityEngine;

public static class JsonUtilityHelper
{
    public static void SaveToJson<T>(string filePath, T data)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }

    public static T LoadFromJson<T>(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(jsonData);
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
            return default(T);
        }
    }
}
