using UnityEngine;
using UnityEngine.UI;

public class PlanetPrefab : MonoBehaviour
{
    public static PlanetPrefab instance; 
    public Text planetNumberText;
    public Sprite planetSprite;
    public int planetNumber;

    void Awake()
    {
        planetNumber = 1;
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        planetNumberText.text = planetNumber.ToString();
    }

    public void OnPlanetClick()
    {
        GameData.instance.saveData.currentLvl = planetNumber;
        GameManager.instance.localLvlNumber = planetNumber;
        GameManager.instance.enemyKiled = GameData.instance.saveData.kiledEnemys[planetNumber - 1];
        PlanetEnemyType();
        GameManager.instance.GenerateNewEnemy();
    }

    void PlanetEnemyType()
    {
        if (planetNumber % 5 == 0)
        {
            if (GameManager.instance.world != null)
            {
                if (GameManager.instance.world.levels[GameManager.instance.levelNumber] != null)
                {
                    GameManager.instance.world.levels[GameManager.instance.levelNumber].levelInfo.enemyType = EnemyType.Boss;
                }
            }
        }
        else
        {
            if (GameManager.instance.world != null)
            {
                if (GameManager.instance.world.levels[GameManager.instance.levelNumber] != null)
                {
                    GameManager.instance.world.levels[GameManager.instance.levelNumber].levelInfo.enemyType = EnemyType.Usual;
                }
            }            
        }
    }
}
