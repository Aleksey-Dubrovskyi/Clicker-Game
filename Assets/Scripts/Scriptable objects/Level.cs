using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [Header("World info")]
    public LevelInfo levelInfo;
}

[System.Serializable]
public class LevelInfo
{
    public string levelName;
    public EnemyType enemyType;
    [Header("Enemy info")]
    public int enemyHP;
    public Sprite[] enemySprites;
    public Sprite[] levelBosses;
    public Sprite planetSprite;
    public Sprite planetBacground;
}

public enum EnemyType
{
    Usual,
    Boss
}
