using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int clickDamage;
    public int autoDamage;
    public int coins;
    public int blasterLvl;
    public int blasterPrice;
    public int blasterDamage;
    public bool[] blasterActiveUpgrades;
    public bool[] lvlCompleted;
    public bool[] activeManagers;
    public int[] managerUpgrades;
    public int[] kiledEnemys;
}

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public SaveData saveData;

    private void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Load();
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerData", FileMode.Create);
        SaveData data = new SaveData();
        data = saveData;
        formatter.Serialize(file, data);
        file.Close();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnDisable()
    {
        Save();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData", FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
        }
        else
        {
            saveData = new SaveData();
            saveData.activeManagers = new bool[100];
            saveData.autoDamage = 0;
            saveData.blasterActiveUpgrades = new bool[100];
            saveData.blasterDamage = 0;
            saveData.blasterLvl = 0;
            saveData.clickDamage = 1;
            saveData.coins = 0;
            saveData.lvlCompleted = new bool[100];
            saveData.managerUpgrades = new int[100];
            saveData.blasterPrice = 5;
        }
    }
}
