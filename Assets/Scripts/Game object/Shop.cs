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
    [SerializeField]
    private GameObject ShopWindow;
    private Animator shopAnim;
    private bool isOpen = false;
    private bool x1, x25, x50, x100, max;
    [SerializeField]
    private Text clickDamage;
    [SerializeField]
    private Text autoDamage;
    [SerializeField]
    private Text blasterDamageText;
    [SerializeField]
    private Text amountOfUpgrades;
    private int blasterLvl;
    [SerializeField]
    private int blasterPrice;
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

    // Start is called before the first frame update
    private void Start()
    {
        shopAnim = ShopWindow.GetComponent<Animator>();
        x1 = false; x25 = true; x50 = true; x100 = true; max = true;
        ManagerInstantiation();
        if (DamageManager.instance != null)
        {
            clickDamage.text = DamageManager.instance.clickDamage.ToString() + "Click damage";
            autoDamage.text = DamageManager.instance.autoDamage.ToString() + "DPS";
            blasterLvl = GameData.instance.saveData.blasterLvl;
            GameData.instance.saveData.blasterDamage = blasterLvl;
            blasterDamageText.text = GameData.instance.saveData.blasterDamage.ToString();
            blasterPrice = GameData.instance.saveData.blasterPrice;
            blasterPriceText.text = blasterPrice.ToString();
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
    }

    public void OnShop()
    {
        CheckForInteractable();
        if (DamageManager.instance.autoDamage > 0)
        {
            DamageManager.instance.AutoDamage();
        }
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
            blasterButton.interactable = true;
            BlasterUpgrade(AmountOfUpgrades());
            GameManager.instance.CoinsUpdate();
            CheckForInteractable();
        }
    }

    public void BlasterUpgrade(int ammountOfUpgrades)
    {
        blasterButtonText.text = "Upgrade";
        blasterLvl += 1 * ammountOfUpgrades;
        blasterPrice *= ammountOfUpgrades;
        GameManager.instance.coins -= blasterPrice;
        GameData.instance.saveData.coins = GameManager.instance.coins;
        blasterPrice = GameData.instance.saveData.blasterPrice;
        blasterPrice += (100/7) * ammountOfUpgrades;
        GameData.instance.saveData.blasterPrice = blasterPrice;
        blasterPriceText.text = GameData.instance.saveData.blasterPrice.ToString();
        GameData.instance.saveData.blasterDamage = blasterLvl;
        blasterDamageText.text = GameData.instance.saveData.blasterDamage.ToString();
        DamageManager.instance.clickDamage = GameData.instance.saveData.blasterDamage;
        GameData.instance.saveData.clickDamage = DamageManager.instance.clickDamage;
        clickDamage.text = DamageManager.instance.clickDamage.ToString() + "Click damage";
        DamageManager.instance.clickDamageInfo.text = GameData.instance.saveData.blasterDamage.ToString();
        GameData.instance.saveData.blasterLvl = blasterLvl;
    }

    private bool CheckForInteractable()
    {
        if (GameManager.instance.coins >= blasterPrice * AmountOfUpgrades())
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
        AmountOfUpgrades();
        CheckForInteractable();
    }

    private int AmountOfUpgrades()
    {
        if (!x25)
        {
            //x1 = true; x25 = true; x50 = false; x100 = true; max = true;
            blasterPriceText.text = "" + blasterPrice * 25;
            amountOfUpgrades.text = "X25";
            return 25;
        }
        else if (!x50)
        {
            //x1 = true; x25 = true; x50 = true; x100 = false; max = true;
            amountOfUpgrades.text = "X50";
            blasterPriceText.text = "" + blasterPrice * 50;
            return 50;
        }
        else if (!x100)
        {
            //x1 = true; x25 = true; x50 = true; x100 = true; max = false;
            amountOfUpgrades.text = "X100";
            blasterPriceText.text = "" + blasterPrice * 100;
            return 100;
        }
        else if (!max)
        {
            //x1 = false; x25 = true; x50 = true; x100 = true; max = true;
            amountOfUpgrades.text = "MAX";

            if (blasterPrice * (GameData.instance.saveData.coins / blasterPrice) <= 0)
            {
                blasterPriceText.text = "" + GameData.instance.saveData.blasterPrice;
                return GameData.instance.saveData.blasterPrice;
            }
            else
            {
                blasterPriceText.text = "" + blasterPrice * (GameData.instance.saveData.coins / blasterPrice);
                return GameData.instance.saveData.coins / blasterPrice;
            }
        }
        else
        {
            //x1 = true; x25 = false; x50 = true; x100 = true; max = true;
            amountOfUpgrades.text = "X1";
            blasterPriceText.text = "" + blasterPrice * 1;
            return 1;
        }
    }

    private void ManagerInstantiation()
    {
        int managerNumber = 0;
        foreach (var manager in Manager.instance.managers)
        {
            GameObject newManager = Instantiate(managerPrefab, managerSection.transform) as GameObject;
            Manager.instance.ColecktingManagerInfo(managerNumber);
            managerNumber++;
        }

    }
}
