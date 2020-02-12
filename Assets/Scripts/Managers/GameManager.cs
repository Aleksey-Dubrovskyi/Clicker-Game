using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Animations
{
    Coin_Flip,
    Teleportation,
    Enemy_appear
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public World world;
    IEnumerator timerCourutine;
    [Header("Level info")]
    public int levelNumber;
    public int localLvlNumber;
    float timeTokillBoss;
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
        }
        coinText.text = coins.ToString();
        StartNewPlanetInstantiation();
        GenerateNewEnemy();

    }

    private Sprite EnemyViewGenerator()
    {
        return currentEnemySprite = world.levels[levelNumber].levelInfo.enemySprites[Random.Range(0, world.levels[levelNumber].levelInfo.enemySprites.Length)];
    }

    private Sprite BossViewGenerator()
    {
        return currentEnemySprite = world.levels[levelNumber].levelInfo.levelBosses[Random.Range(0, world.levels[levelNumber].levelInfo.levelBosses.Length)];
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

    void GenerateUsualEnemy()
    {        
        StopCoroutine("BossCounterCo");
        enemyKiledText.text = GameData.instance.saveData.kiledEnemys[localLvlNumber - 1] + " / " + enemyNeeded;
        enemyAnimation.Play(Animations.Enemy_appear.ToString());
        teleportAnimation.Play(Animations.Teleportation.ToString());
        enemy.sprite = EnemyViewGenerator();
        enemyName.text = currentEnemySprite.name + ", lvl " + (localLvlNumber);
        currentEnemyHP = Mathf.CeilToInt(10 * ((localLvlNumber - 1) + Mathf.Pow(1.55f, localLvlNumber - 1)));
        currentPlanetSprite.sprite = world.levels[levelNumber].levelInfo.planetSprite;
        DamageManager.instance.enemyHP = currentEnemyHP;
        DamageManager.instance.hPText.text = DamageManager.instance.enemyHP.ToString();
        DamageManager.instance.damageTaked = 0;
        DamageManager.instance.hPBar.fillAmount = 0;
    }

    void GenerateBoss()
    {
        timeTokillBoss = 30f;
        enemyAnimation.Play(Animations.Enemy_appear.ToString());
        teleportAnimation.Play(Animations.Teleportation.ToString());
        enemy.sprite = BossViewGenerator();
        enemyName.text = currentEnemySprite.name + ", lvl " + (localLvlNumber);
        currentEnemyHP = Mathf.CeilToInt(10 * ((localLvlNumber - 1) + Mathf.Pow(1.55f, localLvlNumber - 1)) * 10);
        DamageManager.instance.enemyHP = currentEnemyHP;
        DamageManager.instance.hPText.text = DamageManager.instance.enemyHP.ToString();
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
        coinText.text = coins.ToString();
        coinAnimation.Play(Animations.Coin_Flip.ToString());
        Shop.instance.CheckForInteractable();
        Shop.instance.ManagerCheckForInteractable();
        Shop.instance.PriceUpdate();
    }

    public void CoinsUpdate()
    {
        GameData.instance.saveData.coins = coins;
        coinText.text = coins.ToString();
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
                newPlanet.GetComponent<PlanetPrefab>().planetNumber = GameData.instance.saveData.planetNumber[i];
                newPlanet.GetComponent<PlanetPrefab>().planetSprite = planetSpriteList[levelNumber];
            }
        }
    }

    void NewPlanetInstantiation()
    {
        GameObject newPlanet = Instantiate(planetPrefab, planetContainer.transform);
        newPlanet.GetComponent<PlanetPrefab>().planetNumber += localLvlNumber;
        GameData.instance.saveData.planetNumber[localLvlNumber - 1] = newPlanet.GetComponent<PlanetPrefab>().planetNumber;
        newPlanet.GetComponent<PlanetPrefab>().planetSprite = planetSpriteList[levelNumber];
    }
}
