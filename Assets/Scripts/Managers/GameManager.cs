using UnityEngine;
using UnityEngine.UI;

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
    [Header("Level info")]
    public int levelNumber;
    [SerializeField]
    private bool lvlCompleted;
    [SerializeField]
    private int enemyNeeded;
    [SerializeField]
    private int enemyKiled;
    [SerializeField]
    private Text enemyKiledText;
    [SerializeField]
    private Image currentPlanetSprite;
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
    private int coinMultiply;
    [SerializeField]
    private Animator coinAnimation;

    private void Awake()
    {
        instance = this;
        enemyNeeded = 10;
        if (world != null)
        {
            if (world.levels[levelNumber] != null)
            {
                currentEnemyHP = world.levels[levelNumber].levelInfo.enemyHP;
                currentPlanetSprite.sprite = world.levels[levelNumber].levelInfo.planetSprite;
            }
        }
    }

    private void Start()
    {
        if (GameData.instance != null)
        {
            coins = GameData.instance.saveData.coins;
            enemyKiled = GameData.instance.saveData.kiledEnemys[levelNumber];
            enemyKiledText.text = GameData.instance.saveData.kiledEnemys[levelNumber] + " / " + enemyNeeded;
        }
        coinText.text = coins.ToString();
        enemy.sprite = EnemyViewGenerator();
        enemyName.text = currentEnemySprite.name + ", lvl " + (levelNumber + 1);
        GenerateNewEnemy();
    }

    private Sprite EnemyViewGenerator()
    {
        return currentEnemySprite = world.levels[levelNumber].levelInfo.enemySprites[Random.Range(0, world.levels[levelNumber].levelInfo.enemySprites.Length)];
    }

    public void GenerateNewEnemy()
    {
        enemyAnimation.Play(Animations.Enemy_appear.ToString());
        teleportAnimation.Play(Animations.Teleportation.ToString());
        enemy.sprite = EnemyViewGenerator();
        enemyName.text = currentEnemySprite.name + ", lvl " + (levelNumber + 1);
        if (world != null)
        {
            if (world.levels[levelNumber] != null)
            {
                currentEnemyHP = world.levels[levelNumber].levelInfo.enemyHP;
                currentPlanetSprite.sprite = world.levels[levelNumber].levelInfo.planetSprite;
                if (lvlComplete())
                {
                    lvlCompleted = true;
                    GameData.instance.saveData.lvlCompleted[levelNumber] = lvlCompleted;
                }
            }
        }
    }

    public void EarningCoins()
    {
        coins += coinMultiply * (levelNumber + 1);
        GameData.instance.saveData.coins = coins;
        coinText.text = coins.ToString();
        coinAnimation.Play(Animations.Coin_Flip.ToString());
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
        if (enemyKiled < enemyNeeded)
        {
            enemyKiled++;
            GameData.instance.saveData.kiledEnemys[levelNumber] = enemyKiled;
            enemyKiledText.text = GameData.instance.saveData.kiledEnemys[levelNumber] + " / " + enemyNeeded;
        }
        else
        {
            enemyKiled = enemyNeeded;
            enemyKiledText.text = GameData.instance.saveData.kiledEnemys[levelNumber] + " / " + enemyNeeded;
        }

    }
}
