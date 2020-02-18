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
    [SerializeField]
    private int thisManagerLvl;
    public Text managerDps;
    [SerializeField]
    private int thisManagerDPS;
    public Text managerPrice;
    [SerializeField]
    public int thisManagerPrice;
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
        thisManagerPrice = GameData.instance.saveData.managerPrice[managerNumber];
        thisManagerLvl = GameData.instance.saveData.managerLvl[managerNumber];
        thisManagerDPS = GameData.instance.saveData.managerDamage[managerNumber];
        CheckForIntercatableAuto();
    }

    public void ColecktingManagerInfo(int managerNumber)
    {
        managerName.text = managers[managerNumber].managerName;
        managerPrice.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.managerPrice[managerNumber]);
        managerLvl.text = "LVL: " + AbreviationManager.AbbreviateNumber(managers[managerNumber].managerLvl);
        managerDps.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.managerDamage[managerNumber]) + " DPS";
        managerImage.sprite = managers[managerNumber].managerSprite;
        Shop.instance.autoDamage.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.autoDamage) + " DPS";
    }

    public void SavingManagerInfo(int managerNumber)
    {
        GameData.instance.saveData.managerPrice[managerNumber] = managers[managerNumber].managerPrice;
        GameData.instance.saveData.managerLvl[managerNumber] = managers[managerNumber].managerLvl;
        GameData.instance.saveData.managerDamage[managerNumber] = managers[managerNumber].managerDps;
        thisManagerPrice = GameData.instance.saveData.managerPrice[managerNumber];
        thisManagerLvl = GameData.instance.saveData.managerLvl[managerNumber];
        thisManagerDPS = GameData.instance.saveData.managerDamage[managerNumber];
    }

    public void LoadManagerInfo(int managerNumber)
    {
        managers[managerNumber].managerPrice = GameData.instance.saveData.managerPrice[managerNumber];
        managers[managerNumber].managerLvl = GameData.instance.saveData.managerLvl[managerNumber];
        managers[managerNumber].managerDps = GameData.instance.saveData.managerDamage[managerNumber];
    }
    public void ManagerUpgrade()
    {
        float percentToAdd = 0;
        managerButtonText.text = "Upgrade";
        if (SFXManager.instance.isActiveAndEnabled)
            SFXManager.instance.PlaySFX(Clip.Click);
        managers[managerNumber].managerLvl += 1 * Shop.instance.AmountOfUpgrades(managers[managerNumber].managerPrice);
        managers[managerNumber].managerPrice *= Shop.instance.AmountOfUpgrades(managers[managerNumber].managerPrice);
        int repeatNumber = Shop.instance.AmountOfUpgrades(thisManagerPrice);
        for (int i = 0; i < repeatNumber; i++)
        {
            if (!GameData.instance.saveData.activeManagers[managerNumber])
            {
                GameData.instance.saveData.autoDamage += managers[managerNumber].managerDps;
                DamageManager.instance.autoDamage = GameData.instance.saveData.autoDamage;
                GameData.instance.saveData.activeManagers[managerNumber] = true;
                DamageManager.instance.autoDamageInfo.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.autoDamage);
                DamageManager.instance.AutoDamage();
            }
            else
            {
                managers[managerNumber].managerDps += 5;
                GameData.instance.saveData.autoDamage += managers[managerNumber].managerDps;
                DamageManager.instance.autoDamage = GameData.instance.saveData.autoDamage;
                DamageManager.instance.autoDamageInfo.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.autoDamage);
            }
            percentToAdd = ((float)managers[managerNumber].managerPrice / 100) * 7;
            managers[managerNumber].managerPrice += Mathf.CeilToInt(percentToAdd);

        }
        GameManager.instance.coins -= thisManagerPrice;
        GameData.instance.saveData.coins = GameManager.instance.coins;
        SavingManagerInfo(managerNumber);
        ColecktingManagerInfo(managerNumber);
        GameManager.instance.CoinsUpdate();
        Shop.instance.ManagerCheckForInteractable();
    }

    public void CheckForIntercatableAuto()
    {
        if (managerButton != null)
        {
            if (GameData.instance.saveData.coins >= thisManagerPrice * Shop.instance.AmountOfUpgrades(thisManagerPrice))
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
