using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum Animations
{
    Coin_Flip,
    Teleportation,
    Enemy_appear,
    Profit_window_appear,
    Profit_Window_dissappear
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public World world;
    public bool gameStarted;
    private GameObject[] planetsArray;
    [Header("Offline profit")]
    [SerializeField]
    float timeOffline;
    [SerializeField]
    Text timeOfflineText;
    [SerializeField]
    Text offlineProfitText;
    [SerializeField]
    Animator offlineProfit;
    [Header("Level info")]
    public int levelNumber;
    public int localLvlNumber;
    [SerializeField]
    private bool lvlCompleted;
    [SerializeField]
    private int enemyNeeded;
    public int enemyKiled;
    [SerializeField]
    private Text enemyKiledText;
    [SerializeField]
    private Image currentPlanetSprite;
    [SerializeField]
    Image currentBackgroundSprite;
    [SerializeField]
    private GameObject planetContainer;
    [SerializeField]
    private GameObject planetPrefab;
    [SerializeField]
    private Sprite[] planetSpriteList;
    [Header("Enemy info")]
    public int currentEnemyHP;
    [SerializeField]
    private Text enemyName;
    [SerializeField]
    private Image enemy;
    [SerializeField]
    private Sprite currentEnemySprite;
    [SerializeField]
    private Animator teleportAnimation;
    [SerializeField]
    private Animator enemyAnimation;
    [Header("Coin section")]
    [SerializeField]
    private Text coinText;
    public int coins;
    [SerializeField]
    private Animator coinAnimation;

    private void Awake()
    {
        instance = this;
        enemyNeeded = 10;
        localLvlNumber = PlanetPrefab.instance.planetNumber;
        if (world != null)
        {
            if (world.levels[levelNumber] != null)
            {
                currentEnemyHP = Mathf.CeilToInt(10 * ((localLvlNumber - 1) + Mathf.Pow(1.55f, localLvlNumber - 1)));
                currentPlanetSprite.sprite = world.levels[levelNumber].levelInfo.planetSprite;
                currentBackgroundSprite.sprite = world.levels[levelNumber].levelInfo.planetBacground;
            }
        }
    }

    private void Start()
    {
        if (GameData.instance != null)
        {
            coins = GameData.instance.saveData.coins;
            enemyKiled = GameData.instance.saveData.kiledEnemys[localLvlNumber - 1];
            enemyKiledText.text = GameData.instance.saveData.kiledEnemys[localLvlNumber - 1] + " / " + enemyNeeded;
            localLvlNumber = GameData.instance.saveData.currentLvl;
            planetsArray = new GameObject[GameData.instance.saveData.lvlCompleted.Length];
            if (GameData.instance.saveData.firstLaunch == false)
            {
                offlineProfit.SetBool("isOpen", true);
                offlineProfit.Play(Animations.Profit_window_appear.ToString());
            }
        }
        coinText.text = AbreviationManager.AbbreviateNumber(coins);
        StartNewPlanetInstantiation();
        GenerateNewEnemy();
        if (TimeManager.instance != null)
        {
            timeOffline = TimeManager.instance.GetDate();
            var ts = TimeSpan.FromSeconds(timeOffline);
            timeOfflineText.text = string.Format("Your time offline is:\n{0:00}D : {1:00}H : {2:00}M : {3:00}S", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            offlineProfitText.text = "Earned coins:\n" + AbreviationManager.AbbreviateNumber(OfflineEarningCoins());
            CoinsUpdate();
        }

    }

    private Sprite EnemyViewGenerator()
    {
        return currentEnemySprite = world.levels[levelNumber].levelInfo.enemySprites[UnityEngine.Random.Range(0, world.levels[levelNumber].levelInfo.enemySprites.Length)];
    }

    private Sprite BossViewGenerator()
    {
        return currentEnemySprite = world.levels[levelNumber].levelInfo.levelBosses[UnityEngine.Random.Range(0, world.levels[levelNumber].levelInfo.levelBosses.Length)];
    }

    public void GenerateNewEnemy()
    {
        if (world != null)
        {
            if (world.levels[levelNumber] != null)
            {
                if (world.levels[levelNumber].levelInfo.enemyType == EnemyType.Usual)
                {
                    GenerateUsualEnemy();
                }
                else if (world.levels[levelNumber].levelInfo.enemyType == EnemyType.Boss)
                {
                    GenerateBoss();
                }

                if (lvlComplete())
                {
                    if (GameData.instance.saveData.lvlCompleted[localLvlNumber - 1] != true)
                    {
                        lvlCompleted = true;
                        GameData.instance.saveData.lvlCompleted[localLvlNumber - 1] = lvlCompleted;
                        NewPlanetInstantiation();
                    }
                }
            }
        }
    }

    private void GenerateUsualEnemy()
    {
        StopCoroutine("BossCounterCo");
        enemyKiledText.text = GameData.instance.saveData.kiledEnemys[localLvlNumber - 1] + " / " + enemyNeeded;
        enemyAnimation.Play(Animations.Enemy_appear.ToString());
        if (SFXManager.instance.isActiveAndEnabled)
            SFXManager.instance.PlaySFX(Clip.Teleport);
        teleportAnimation.Play(Animations.Teleportation.ToString());
        enemy.sprite = EnemyViewGenerator();
        enemyName.text = currentEnemySprite.name + ", lvl " + (localLvlNumber);
        currentEnemyHP = Mathf.CeilToInt(10 * ((localLvlNumber - 1) + Mathf.Pow(1.55f, localLvlNumber - 1)));
        currentPlanetSprite.sprite = world.levels[levelNumber].levelInfo.planetSprite;
        currentBackgroundSprite.sprite = world.levels[levelNumber].levelInfo.planetBacground;
        DamageManager.instance.enemyHP = currentEnemyHP;
        DamageManager.instance.hPText.text = AbreviationManager.AbbreviateNumber(DamageManager.instance.enemyHP);
        DamageManager.instance.damageTaked = 0;
        DamageManager.instance.hPBar.fillAmount = 0;
    }

    private void GenerateBoss()
    {
        enemyAnimation.Play(Animations.Enemy_appear.ToString());
        if (SFXManager.instance.isActiveAndEnabled)
            SFXManager.instance.PlaySFX(Clip.Teleport);
        teleportAnimation.Play(Animations.Teleportation.ToString());
        enemy.sprite = BossViewGenerator();
        enemyName.text = currentEnemySprite.name + ", lvl " + (localLvlNumber);
        currentEnemyHP = Mathf.CeilToInt(10 * ((localLvlNumber - 1) + Mathf.Pow(1.55f, localLvlNumber - 1)) * 10);
        DamageManager.instance.enemyHP = currentEnemyHP;
        DamageManager.instance.hPText.text = AbreviationManager.AbbreviateNumber(DamageManager.instance.enemyHP);
        DamageManager.instance.damageTaked = 0;
        DamageManager.instance.hPBar.fillAmount = 0;
        StartCoroutine("BossCounterCo");
    }

    private IEnumerator BossCounterCo()
    {
        float timer = 30f;
        while (timer > 0)
        {
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
            enemyKiledText.text = timer.ToString("0.00");
            if (timer < 0)
            {
                GenerateNewEnemy();
            }
        }

    }

    public void EarningCoins()
    {
        coins += Mathf.CeilToInt((float)currentEnemyHP / 15);
        GameData.instance.saveData.coins = coins;
        coinText.text = AbreviationManager.AbbreviateNumber(coins);
        coinAnimation.Play(Animations.Coin_Flip.ToString());
        Shop.instance.CheckForInteractable();
        Shop.instance.ManagerCheckForInteractable();
        Shop.instance.PriceUpdate();
    }

    int OfflineEarningCoins()
    {
        int earnedCoins = 0;
        if (GameData.instance.saveData.autoDamage != 0)
        {
            if (timeOffline < 86400 && GameData.instance.saveData.firstLaunch == false && timeOffline > (currentEnemyHP / GameData.instance.saveData.autoDamage))
            {
                float secondsToOneEnemy = currentEnemyHP / GameData.instance.saveData.autoDamage;
                float killedEnemys = timeOffline / Mathf.CeilToInt(secondsToOneEnemy);
                earnedCoins = Mathf.CeilToInt(((float)currentEnemyHP / 15) * killedEnemys);
                coins += earnedCoins;
                GameData.instance.saveData.coins = coins;
                return earnedCoins;
            }
            else
            {
                return 0;
            }
        }
        if (GameData.instance.saveData.clickDamage != 0)
        {
            float secondsToOneEnemy = currentEnemyHP / GameData.instance.saveData.clickDamage;
            float killedEnemys = timeOffline / Mathf.CeilToInt(secondsToOneEnemy);
            earnedCoins = Mathf.CeilToInt(((float)currentEnemyHP / 15) * killedEnemys);
            coins += earnedCoins;
            GameData.instance.saveData.coins = coins;
            return earnedCoins;
        }
        else
        {
            return 0;
        }


    }

    public void CoinsUpdate()
    {
        GameData.instance.saveData.coins = coins;
        coinText.text = AbreviationManager.AbbreviateNumber(coins);
        Shop.instance.PriceUpdate();
    }

    public bool lvlComplete()
    {
        if (enemyKiled >= enemyNeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EnemyKiled()
    {
        enemyKiled = GameData.instance.saveData.kiledEnemys[localLvlNumber - 1];
        if (world.levels[levelNumber].levelInfo.enemyType == EnemyType.Boss)
        {
            if (GameData.instance.saveData.lvlCompleted[localLvlNumber - 1] != true)
            {
                lvlCompleted = true;
                GameData.instance.saveData.lvlCompleted[localLvlNumber - 1] = lvlCompleted;
                NewPlanetInstantiation();
            }
        }
        else if (enemyKiled < enemyNeeded)
        {
            enemyKiled++;
            GameData.instance.saveData.kiledEnemys[localLvlNumber - 1] = enemyKiled;
            enemyKiledText.text = GameData.instance.saveData.kiledEnemys[localLvlNumber - 1] + " / " + enemyNeeded;
        }
        else
        {
            enemyKiled = enemyNeeded;
            enemyKiledText.text = GameData.instance.saveData.kiledEnemys[localLvlNumber - 1] + " / " + enemyNeeded;
        }

    }

    private void StartNewPlanetInstantiation()
    {
        for (int i = 0; i < GameData.instance.saveData.lvlCompleted.Length; i++)
        {
            if (GameData.instance.saveData.lvlCompleted[i])
            {
                GameObject newPlanet = Instantiate(planetPrefab, planetContainer.transform);
                planetsArray[i] = newPlanet;
                planetsArray[i].GetComponent<PlanetPrefab>().planetNumber = GameData.instance.saveData.planetNumber[i];
                planetsArray[i].GetComponent<PlanetPrefab>().LevelSelect();
                planetsArray[i].GetComponent<PlanetPrefab>().planetSprite = planetSpriteList[levelNumber];
            }
        }
    }

    private void NewPlanetInstantiation()
    {
        GameObject newPlanet = Instantiate(planetPrefab, planetContainer.transform);
        newPlanet.GetComponent<PlanetPrefab>().planetNumber += localLvlNumber;
        GameData.instance.saveData.planetNumber[localLvlNumber - 1] = newPlanet.GetComponent<PlanetPrefab>().planetNumber;
        newPlanet.GetComponent<PlanetPrefab>().planetSprite = planetSpriteList[levelNumber];
    }

    public void OKbutton()
    {
        if (SFXManager.instance.isActiveAndEnabled)
            SFXManager.instance.PlaySFX(Clip.Click);
        offlineProfit.Play(Animations.Profit_Window_dissappear.ToString());
        offlineProfit.SetBool("isOpen", false);
    }

    private void OnApplicationQuit()
    {
        TimeManager.instance.SaveDate();
        GameData.instance.saveData.firstLaunch = false;
    }
}
