using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    private static string GetSavePath()
    {
        return Application.persistentDataPath + "/player.sav";
    }

    public static void SaveGame(PlayerData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetSavePath();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadGame()
    {
        string path = GetSavePath();
        if (File.Exists(path))
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load save file. It might be corrupted. Error: " + e.Message);
                return null;
            }
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static bool SaveFileExists()
    {
        return File.Exists(GetSavePath());
    }
}

