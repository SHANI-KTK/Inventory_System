using System.IO;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;

public class Data : MonoBehaviour
{
    public void LoadData<T>(ref T obj, string fileName)
    {
        string filePath = Application.persistentDataPath + "/" + fileName + ".json";
        if (File.Exists(filePath))
        {
            string jsondata = File.ReadAllText(filePath);

            if (jsondata == "")
            {
                SaveData(obj, fileName);
                return;
            }
            if (obj.GetType().IsArray)
                obj = FromJson<T>(jsondata);
            else
                JsonUtility.FromJsonOverwrite(jsondata, obj);
        }
        else
        {
            SaveData(obj, fileName);
        }
    }
    public void SaveData<T>(T obj, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        string jsondata = obj.GetType().IsArray ? ToJson(obj) : JsonUtility.ToJson(obj);
        if (!string.IsNullOrEmpty(jsondata))
            File.WriteAllText(path, jsondata);
    }

    #region Json Helper
    public static T FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T Items;
    }
    #endregion
}
