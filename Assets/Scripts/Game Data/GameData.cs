using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveData
{
    public long clickDamage;
    public long autoDamage;
    public long coins;
    public int blasterLvl;
    public long blasterPrice;
    public long blasterDamage;
    public int currentLvl;
    public bool firstLaunch;
    //public bool[] blasterActiveUpgrades;
    public bool[] lvlCompleted;
    public bool[] activeManagers;
    public int[] planetNumber;
    public long[] managerLvl;
    public long[] managerPrice;
    public long[] managerDamage;
    //public int[] managerUpgrades;
    public int[] kiledEnemys;
}

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public SaveData saveData;

    private void Awake()
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
        FileStream file = File.Open(Application.persistentDataPath + "/playerData", FileMode.Create, FileAccess.Write);
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
            FileStream file = File.Open(Application.persistentDataPath + "/playerData", FileMode.Open, FileAccess.Read);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
        }
        else
        {
            saveData = new SaveData();
            saveData.activeManagers = new bool[10];
            saveData.autoDamage = 0;
            //saveData.blasterActiveUpgrades = new bool[100];
            saveData.blasterDamage = 1;
            saveData.blasterLvl = 1;
            saveData.clickDamage = 1;
            saveData.coins = 0;
            saveData.lvlCompleted = new bool[100];
            saveData.planetNumber = new int[100];
            //saveData.managerUpgrades = new int[100];
            saveData.blasterPrice = 5;
            saveData.managerPrice = new long[10];
            saveData.managerLvl = new long[10];
            saveData.managerDamage = new long[10];
            saveData.currentLvl = 1;
            saveData.kiledEnemys = new int[100];
            saveData.firstLaunch = true;
        }
    }
}
