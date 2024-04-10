using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(GameManager gameManager) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/i_love.you";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gameManager);
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Saved...");
    }

    public static PlayerData LoadPlayer() {
        string path = Application.persistentDataPath + "/i_love.you";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            Debug.Log("Loaded");
            return data;
        }
        else {
            Debug.Log("File not found in " +path);
            return null;
        }
    }
}
