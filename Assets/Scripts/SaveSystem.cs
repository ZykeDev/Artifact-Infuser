using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System;

public static class SaveSystem
{
    private static string path;
    private static readonly string defaultSavefile = "/data.save";
    
    // Import JS functions to save the game through the browser
    [DllImport("__Internal")] private static extern void SaveGame(string saveData);
    [DllImport("__Internal")] private static extern string LoadGame();
    [DllImport("__Internal")] private static extern void ClearGameSave();


    public static void Save(SaveData saveData)
    {
#if UNITY_WEBGL
        SaveWeb(saveData);
#else
        path = Application.persistentDataPath + defaultSavefile;
        SaveLocal(saveData);
#endif
    }
    
    public static SaveData Load()
    {
#if UNITY_WEBGL
        return LoadWeb();
#else
        return LoadLocal();
#endif
    }

    private static SaveData LoadLocal()
    {
        if (path == "" || path == null)
        {
            path = Application.persistentDataPath + defaultSavefile;
        }
        
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData saveData = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return saveData;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("No save file found.");
#endif
            return null;
        }
    }

    private static SaveData LoadWeb()
    {
        string jsonSaveData = LoadGame();

        // Return null if no save was found
        if (jsonSaveData == null || jsonSaveData == "") return null;

        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonSaveData);
        
        return saveData;

        /* Deprecated
        // Convert the base64 into a byte array
        byte[] bytes = Convert.FromBase64String(websaveData);

        // Read the bytes as a stream and deserialize them
        MemoryStream stream = new MemoryStream(bytes);
        BinaryFormatter formatter = new BinaryFormatter();

        SaveData saveData = formatter.Deserialize(stream) as SaveData;
        
        return saveData;
        */
    }


    /// <summary>
    /// Saves the game in a local file
    /// </summary>
    /// <param name="saveData"></param>
    private static void SaveLocal(SaveData saveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        // Serialize the save data into a binary file
        formatter.Serialize(stream, saveData);
        stream.Close();
    }


    /// <summary>
    /// Saves the game in the browser's LocalStorage
    /// </summary>
    /// <param name="saveData"></param>
    private static void SaveWeb(SaveData saveData)
    {
        string jsonSaveData = JsonUtility.ToJson(saveData);
        SaveGame(jsonSaveData);

        /* Deprecated
        MemoryStream stream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();

        // Serialize the save data into a base64 string
        formatter.Serialize(stream, saveData);
        string base64save = Convert.ToBase64String(stream.ToArray());

        SaveGame(base64save);
        */
    }


    /// <summary>
    /// Returns the serialized savefile for personal storage
    /// </summary>
    /// <param name="saveData"></param>
    /// <returns></returns>
    public static string GetSaveFile(SaveData saveData)
    {
        // TODO
        return "";
    }


    public static void ClearSave() => ClearGameSave();
}
