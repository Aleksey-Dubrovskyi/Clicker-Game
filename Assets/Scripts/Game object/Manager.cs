using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ManagerInfo
{
    public string managerName;
    public int managerLvl;
    public int managerDps;
    public Sprite managerSprite;
    public ManagerUpgrades[] upgrades;
}

[System.Serializable]
public class ManagerUpgrades
{
    public string upgradeName;
    public float upgradeBonus;
    public string upgradeDescription;
    public bool isLocked;
    public bool isActive;
    public Sprite upgradeSprite;
}

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public ManagerInfo[] managers;
    public Text managerName;
    public Text managerLvl;
    public Text managerDps;
    public Image managerImage;

    private void Awake()
    {
        instance = this;
    }

    public void ColecktingManagerInfo(int managerNumber)
    {
        managerName.text = managers[managerNumber].managerName;
        managerLvl.text = "LVL: " + managers[managerNumber].managerLvl.ToString();
        managerDps.text = managers[managerNumber].managerDps.ToString() + " DPS";
        managerImage.sprite = managers[managerNumber].managerSprite;
    }
}
