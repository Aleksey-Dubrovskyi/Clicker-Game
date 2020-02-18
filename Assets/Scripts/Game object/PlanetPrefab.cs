using UnityEngine;
using UnityEngine.UI;

public class PlanetPrefab : MonoBehaviour
{
    public static PlanetPrefab instance;
    public Text planetNumberText;
    public Sprite planetSprite;
    public int planetNumber;

    private void Awake()
    {
        planetNumber = 1;
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        planetNumberText.text = planetNumber.ToString();
        gameObject.GetComponent<Image>().sprite = planetSprite;
    }

    public void OnPlanetClick()
    {
        GameData.instance.saveData.currentLvl = planetNumber;
        GameManager.instance.localLvlNumber = planetNumber;
        GameManager.instance.enemyKiled = GameData.instance.saveData.kiledEnemys[planetNumber - 1];
        PlanetEnemyType();
        GameManager.instance.GenerateNewEnemy();
    }

    private void PlanetEnemyType()
    {
        LevelSelect();
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
        if (planetNumber % 10 == 0)
        {
            if (GameManager.instance.world != null)
            {
                GameManager.instance.levelNumber = planetNumber / 10;
                if (GameManager.instance.world.levels[GameManager.instance.levelNumber] != null)
                {
                    GameManager.instance.world.levels[GameManager.instance.levelNumber].levelInfo.enemyType = EnemyType.Boss;
                }
            }
        }

    }

    private int Devider()
    {
        string numberLenght = planetNumber.ToString();
        switch (numberLenght.Length)
        {
            case 1:
            case 2:
                return 10;
            case 3:
                return 100;
            case 4:
                return 1000;
            case 5:
                return 10000;
            case 6:
                return 100000;
            case 7:
                return 1000000;
            case 8:
                return 10000000;
            case 9:
                return 100000000;
            default:
                return 0;
        }
    }

    public void LevelSelect()
    {
        int levelSelect;
        if (planetNumber % 10 == 0)
        {
            levelSelect = (planetNumber - 1) / Devider();
        }
        else
        {
            levelSelect = (planetNumber - (planetNumber % 10)) / Devider();
        }

        switch (levelSelect)
        {
            case 1:
                GameManager.instance.levelNumber = 1;
                break;
            case 2:
                GameManager.instance.levelNumber = 2;
                break;
            case 3:
                GameManager.instance.levelNumber = 3;
                break;
            case 4:
                GameManager.instance.levelNumber = 4;
                break;
            case 5:
                GameManager.instance.levelNumber = 5;
                break;
            case 6:
                GameManager.instance.levelNumber = 6;
                break;
            case 7:
                GameManager.instance.levelNumber = 8;
                break;
            case 9:
                GameManager.instance.levelNumber = 9;
                break;
            case 0:
                GameManager.instance.levelNumber = 0;
                break;
            default:
                break;
        }
    }
}
