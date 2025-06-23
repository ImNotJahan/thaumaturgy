using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class JsonDataService
{
    public static void SaveData(object data, string relativePath, string fileName)
    {
        string path = Application.persistentDataPath + "/" + relativePath;
        string stringData = JsonConvert.SerializeObject(data);

        Directory.CreateDirectory(path);

        path += "/" + fileName;

        Debug.Log(path);

        if (File.Exists(path)) File.Delete(path);

        using FileStream stream = File.Create(path);
        stream.Close();

        File.WriteAllText(path, stringData);
    }

    public static T LoadData<T>(string relativePath, string fileName)
    {
        string path = Application.persistentDataPath + "/" + relativePath + "/" + fileName;

        return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
    }
}
