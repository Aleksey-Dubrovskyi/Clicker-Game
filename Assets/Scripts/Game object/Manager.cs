using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ManagerInfo
{
    public string managerName;
    public int managerLvl;
    public int managerDps;
    public int managerPrice;
    public Sprite managerSprite;
    //public ManagerUpgrades[] upgrades;
}

//Ability to make manager upgrades. Not in release version
//[System.Serializable]
//public class ManagerUpgrades
//{
//    public string upgradeName;
//    public float upgradeBonus;
//    public string upgradeDescription;
//    public bool isLocked;
//    public bool isActive;
//    public Sprite upgradeSprite;
//}

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public ManagerInfo[] managers;
    public Text managerName;
    public Text managerLvl;
    public Text managerDps;
    public Text managerPrice;
    public Text managerButtonText;
    public Button managerButton;
    public Image managerImage;
    public int managerNumber = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CheckForIntercatableAuto();
    }

    public void ColecktingManagerInfo(int managerNumber)
    {
        managerName.text = managers[managerNumber].managerName;
        managerPrice.text = managers[managerNumber].managerPrice.ToString();
        managerLvl.text = "LVL: " + managers[managerNumber].managerLvl.ToString();
        managerDps.text = managers[managerNumber].managerDps.ToString() + " DPS";
        managerImage.sprite = managers[managerNumber].managerSprite;
        Shop.instance.autoDamage.text = GameData.instance.saveData.autoDamage.ToString() + " DPS";
    }

    public void SavingManagerInfo(int managerNumber)
    {
        GameData.instance.saveData.managerPrice[managerNumber] = managers[managerNumber].managerPrice;
        GameData.instance.saveData.managerLvl[managerNumber] = managers[managerNumber].managerLvl;
        GameData.instance.saveData.managerDamage[managerNumber] = managers[managerNumber].managerDps;
    }

    public void LoadManagerInfo(int managerNumber)
    {
        managers[managerNumber].managerPrice = GameData.instance.saveData.managerPrice[managerNumber];
        managers[managerNumber].managerLvl = GameData.instance.saveData.managerLvl[managerNumber];
        managers[managerNumber].managerDps = GameData.instance.saveData.managerDamage[managerNumber];
    }
    public void ManagerUpgrade()
    {
        CheckForIntercatableAuto();
        float percentToAdd = 0;
        managerButtonText.text = "Upgrade";
        managers[managerNumber].managerLvl += 1 * Shop.instance.AmountOfUpgrades();
        managers[managerNumber].managerPrice *= Shop.instance.AmountOfUpgrades();
        GameManager.instance.coins -= managers[managerNumber].managerPrice;
        GameData.instance.saveData.coins = GameManager.instance.coins;
        for (int i = 0; i < Shop.instance.AmountOfUpgrades(); i++)
        {
            percentToAdd = ((float)managers[managerNumber].managerPrice / 100) * 7;
            managers[managerNumber].managerPrice += Mathf.CeilToInt(percentToAdd);
        }
        if (!GameData.instance.saveData.activeManagers[managerNumber])
        {            
            GameData.instance.saveData.autoDamage += managers[managerNumber].managerDps;
            DamageManager.instance.autoDamage = GameData.instance.saveData.autoDamage;
            GameData.instance.saveData.activeManagers[managerNumber] = true;
            DamageManager.instance.autoDamageInfo.text = GameData.instance.saveData.autoDamage.ToString();
            DamageManager.instance.AutoDamage();
        }
        else
        {
            managers[managerNumber].managerDps += 5;
            GameData.instance.saveData.autoDamage += managers[managerNumber].managerDps;
            DamageManager.instance.autoDamageInfo.text = GameData.instance.saveData.autoDamage.ToString();
        }

        SavingManagerInfo(managerNumber);
        ColecktingManagerInfo(managerNumber);
        GameManager.instance.CoinsUpdate();
        CheckForIntercatableAuto();
    }

    public void CheckForIntercatableAuto()
    {
        if (managerButton != null)
        {
            if (GameData.instance.saveData.coins >= managers[managerNumber].managerPrice * Shop.instance.AmountOfUpgrades())
            {
                managerButton.interactable = true;
            }
            else
            {
                managerButton.interactable = false;
            }
        }
    }
}
