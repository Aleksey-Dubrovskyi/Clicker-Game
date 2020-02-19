using UnityEngine;
using UnityEngine.UI;

public enum DamageType
{
    ClickDamage,
    AutoDamage
}

public class Shop : MonoBehaviour
{
    public DamageType damageType;
    public static Shop instance;
    [SerializeField]
    private GameObject ShopWindow;
    private Animator shopAnim;
    private bool isOpen = false;
    private bool x1, x25, x50, x100, max;
    [SerializeField]
    private Text clickDamage;
    public Text autoDamage;
    [SerializeField]
    [Header("Blaster section")]
    private Text blasterDamageText;
    [SerializeField]
    private Text amountOfUpgrades;
    private int blasterLvl;
    [SerializeField]
    private Text blasterLvlText;
    [SerializeField]
    private long blasterPrice;
    [SerializeField]
    private Text blasterPriceText;
    [SerializeField]
    private Text blasterButtonText;
    [SerializeField]
    private Button blasterButton;
    [SerializeField]
    private GameObject managerPrefab;
    [SerializeField]
    private GameObject managerSection;
    private GameObject[] managerArray;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        shopAnim = ShopWindow.GetComponent<Animator>();
        x1 = false; x25 = true; x50 = true; x100 = true; max = true;
        blasterLvlText.text = "LVL: " + AbreviationManager.AbbreviateNumber(GameData.instance.saveData.blasterLvl);
        managerArray = new GameObject[Manager.instance.managers.Length];
        ManagerInstantiation();
        if (DamageManager.instance != null)
        {
            clickDamage.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.clickDamage) + " Click damage";
            autoDamage.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.autoDamage) + " DPS";
            blasterLvl = GameData.instance.saveData.blasterLvl;
            GameData.instance.saveData.blasterDamage = blasterLvl;
            blasterDamageText.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.blasterDamage);
            blasterPrice = GameData.instance.saveData.blasterPrice;
            blasterPriceText.text = AbreviationManager.AbbreviateNumber(blasterPrice);
        }

        if (blasterLvl <= 0)
        {
            blasterButtonText.text = "Buy";
        }
        else
        {
            blasterButtonText.text = "Upgrade";
        }
        CheckForInteractable();
        ManagerCheckForInteractable();
    }

    public void OnShop()
    {
        if (SFXManager.instance.isActiveAndEnabled)
        {
            SFXManager.instance.PlaySFX(Clip.Click);
        }

        CheckForInteractable();
        ManagerCheckForInteractable();
        if (isOpen == false)
        {
            shopAnim.SetBool("Out", true);
            isOpen = true;
        }
        else if (isOpen == true)
        {
            shopAnim.SetBool("Out", false);
            isOpen = false;
        }
    }

    public void BuyAndUpgrade()
    {
        if (CheckForInteractable())
        {
            if (SFXManager.instance.isActiveAndEnabled)
            {
                SFXManager.instance.PlaySFX(Clip.Click);
            }

            blasterButton.interactable = true;
            BlasterUpgrade(AmountOfUpgrades(blasterPrice));
            GameManager.instance.CoinsUpdate();
            CheckForInteractable();
        }
    }

    public void BlasterUpgrade(long ammountOfUpgrades)
    {
        float percentToAdd = 0;
        blasterButtonText.text = "Upgrade";
        blasterLvl += 1 * (int)ammountOfUpgrades;
        blasterLvlText.text = "LVL: " + AbreviationManager.AbbreviateNumber(blasterLvl);
        blasterPrice *= ammountOfUpgrades;
        GameManager.instance.coins -= blasterPrice;
        GameData.instance.saveData.coins = GameManager.instance.coins;
        blasterPrice = GameData.instance.saveData.blasterPrice;
        for (int i = 0; i < ammountOfUpgrades; i++)
        {
            percentToAdd = ((float)blasterPrice / 100) * 7;
            blasterPrice += Mathf.CeilToInt(percentToAdd);
        }
        GameData.instance.saveData.blasterPrice = blasterPrice;
        blasterPriceText.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.blasterPrice);
        GameData.instance.saveData.blasterDamage = blasterLvl;
        blasterDamageText.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.blasterDamage);
        DamageManager.instance.clickDamage = GameData.instance.saveData.blasterDamage;
        GameData.instance.saveData.clickDamage = DamageManager.instance.clickDamage;
        clickDamage.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.clickDamage) + " Click damage";
        DamageManager.instance.clickDamageInfo.text = AbreviationManager.AbbreviateNumber(GameData.instance.saveData.blasterDamage);
        GameData.instance.saveData.blasterLvl = blasterLvl;
    }

    public bool CheckForInteractable()
    {
        if (GameManager.instance.coins >= blasterPrice * AmountOfUpgrades(blasterPrice))
        {
            blasterButton.interactable = true;
            return true;
        }
        else
        {
            blasterButton.interactable = false;
            return false;
        }
    }

    public void OnAmountButton()
    {
        if (SFXManager.instance.isActiveAndEnabled)
        {
            SFXManager.instance.PlaySFX(Clip.Click);
        }

        if (!x25)
        {
            x1 = true; x25 = true; x50 = false; x100 = true; max = true;
        }
        else if (!x50)
        {
            x1 = true; x25 = true; x50 = true; x100 = false; max = true;
        }
        else if (!x100)
        {
            x1 = true; x25 = true; x50 = true; x100 = true; max = false;
        }
        else if (!max)
        {
            x1 = false; x25 = true; x50 = true; x100 = true; max = true;
        }
        else
        {
            x1 = true; x25 = false; x50 = true; x100 = true; max = true;
        }
        PriceUpdate();
        CheckForInteractable();
        ManagerCheckForInteractable();
    }

    public void PriceUpdate()
    {
        BlasterPriceOfupdate();
        ManagerAmountOfUpgrades();
    }

    public long AmountOfUpgrades(long unitPrcie)
    {
        if (!x25)
        {
            //x1 = true; x25 = true; x50 = false; x100 = true; max = true;
            amountOfUpgrades.text = "X25";
            return 25;
        }
        else if (!x50)
        {
            //x1 = true; x25 = true; x50 = true; x100 = false; max = true;
            amountOfUpgrades.text = "X50";
            return 50;
        }
        else if (!x100)
        {
            //x1 = true; x25 = true; x50 = true; x100 = true; max = false;
            amountOfUpgrades.text = "X100";
            return 100;
        }
        else if (!max)
        {
            //x1 = false; x25 = true; x50 = true; x100 = true; max = true;
            amountOfUpgrades.text = "MAX";

            if (unitPrcie * (GameData.instance.saveData.coins / unitPrcie) <= 0)
            {
                return 1;
            }
            else
            {
                return GameData.instance.saveData.coins / unitPrcie;
            }
        }
        else
        {
            //x1 = true; x25 = false; x50 = true; x100 = true; max = true;
            amountOfUpgrades.text = "X1";
            return 1;
        }

    }

    private void BlasterPriceOfupdate()
    {
        blasterPriceText.text = AbreviationManager.AbbreviateNumber(blasterPrice * AmountOfUpgrades(GameData.instance.saveData.blasterPrice));
    }

    private void ManagerAmountOfUpgrades()
    {
        for (int i = 0; i < managerArray.Length; i++)
        {
            managerArray[i].GetComponent<Manager>().managerPrice.text = AbreviationManager.AbbreviateNumber(managerArray[i].GetComponent<Manager>().thisManagerPrice * AmountOfUpgrades(managerArray[i].GetComponent<Manager>().thisManagerPrice));
        }
        //Manager.instance.managerPrice.text = "" + Manager.instance.thisManagerPrice * AmountOfUpgrades(Manager.instance.thisManagerPrice);
    }

    public void ManagerCheckForInteractable()
    {
        for (int i = 0; i < managerArray.Length; i++)
        {
            Manager manager = managerArray[i].GetComponent<Manager>();
            if (manager.managerButton != null)
            {
                if (GameData.instance.saveData.coins >= manager.thisManagerPrice * Shop.instance.AmountOfUpgrades(manager.thisManagerPrice))
                {
                    manager.managerButton.interactable = true;
                }
                else
                {
                    manager.managerButton.interactable = false;
                }
            }
        }

    }

    private void ManagerInstantiation()
    {
        int managerNumber = 0;
        foreach (var manager in Manager.instance.managers)
        {
            GameObject newManager = Instantiate(managerPrefab, managerSection.transform) as GameObject;
            Manager.instance.SavingManagerInfo(managerNumber);
            if (GameData.instance != null)
            {
                Manager.instance.LoadManagerInfo(managerNumber);
            }
            Manager.instance.ColecktingManagerInfo(managerNumber);
            Manager.instance.managerNumber = managerNumber;
            managerArray[managerNumber] = newManager;
            managerNumber++;

        }
    }
}
