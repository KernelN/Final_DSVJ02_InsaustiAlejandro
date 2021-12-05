using UnityEngine;

public static class JsonFileManager<T>
{
    public static void SaveDataToFile(T objectToSave, string dataPath)
    {
        string dataJSON = JsonUtility.ToJson(objectToSave);
        FileManager<string>.SaveDataToFile(dataJSON, dataPath);
    }
    public static T LoadDataFromFile(T jsonGetter, string dataPath)
    {
        string dataJSON;
        dataJSON = FileManager<string>.LoadDataFromFile(dataPath);
        JsonUtility.FromJsonOverwrite(dataJSON, jsonGetter);
        return jsonGetter;
    }
}