using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private DateTime currentDate;
    private DateTime oldDate;

    public string saveLocation;
    public static TimeManager instance;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

        saveLocation = "LastSaveDate";
    }


    public float GetDate()
    {
        currentDate = DateTime.UtcNow;

        string tempLocation = PlayerPrefs.GetString(saveLocation, "1");

        long tempTime = Convert.ToInt64(tempLocation);

        oldDate = DateTime.FromBinary(tempTime);

        TimeSpan difference = currentDate.Subtract(oldDate);

        return (float)difference.TotalSeconds;
    }

    public void SaveDate()
    {
        PlayerPrefs.SetString(saveLocation, DateTime.UtcNow.ToBinary().ToString());
    }
}
