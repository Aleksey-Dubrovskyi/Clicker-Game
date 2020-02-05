using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageManager : MonoBehaviour
{
    public static DamageManager instance;
    public World world;
    public int clickDamage;
    public int autoDamage;
    private int enemyHP;
    private int damageTaked;
    [SerializeField]
    private Image hPBar;
    [SerializeField]
    private Text hPText;
    public Text clickDamageInfo;
    [SerializeField]
    private GameObject damageTextPrefab;
    [SerializeField]
    private GameObject clickDamageTextContainer;
    public Text autoDamageInfo;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (GameData.instance != null)
        {
            clickDamage = GameData.instance.saveData.clickDamage;
            autoDamage = GameData.instance.saveData.autoDamage;
        }
        if (autoDamage > 0)
        {
            AutoDamage();
        }
        if (GameManager.instance != null)
        {
            enemyHP = GameManager.instance.currentEnemyHP;
        }

        hPText.text = enemyHP.ToString();
        clickDamageInfo.text = "" + clickDamage;
        autoDamageInfo.text = "" + autoDamage;
    }

    public void ClickDamage()
    {
        EnemyIsDead();
        if (GameManager.instance != null && !EnemyIsDead())
        {
            enemyHP -= clickDamage;
            damageTaked += clickDamage;
            Enemy.instance.anim.Play("Enemy_damage");
            HPBar();
            HPText();
            DamageTextAppear(clickDamage);
        }
        if (EnemyIsDead())
        {
            GameManager.instance.EarningCoins();
            enemyHP = GameManager.instance.currentEnemyHP;
            hPText.text = enemyHP.ToString();
            damageTaked = 0;
            hPBar.fillAmount = 0;
            GameManager.instance.GenerateNewEnemy();
            GameManager.instance.EnemyKiled();
            if (!GameData.instance.saveData.lvlCompleted[GameManager.instance.levelNumber])
            {
                GameData.instance.saveData.lvlCompleted[GameManager.instance.levelNumber] = true;

            }
        }
    }

    public void AutoDamage()
    {
        StartCoroutine(AutoDamageCo());
    }

    private IEnumerator AutoDamageCo()
    {
        while (Application.isPlaying)
        {
            yield return new WaitForSeconds(1f);
            EnemyIsDead();
            if (GameManager.instance != null && !EnemyIsDead())
            {
                enemyHP -= autoDamage;
                damageTaked += autoDamage;
                Enemy.instance.anim.Play("Enemy_damage");
                HPBar();
                HPText();
                DamageTextAppear(autoDamage);
            }
            if (EnemyIsDead())
            {
                GameManager.instance.EarningCoins();
                enemyHP = GameManager.instance.currentEnemyHP;
                hPText.text = enemyHP.ToString();
                damageTaked = 0;
                hPBar.fillAmount = 0;
                GameManager.instance.GenerateNewEnemy();
            }
        }
    }

    public void HPBar()
    {
        if (hPBar != null)
        {
            hPBar.fillAmount = damageTaked / (float)GameManager.instance.currentEnemyHP;
        }
    }

    public void HPText()
    {
        if (hPText != null)
        {
            hPText.text = enemyHP.ToString();
        }
    }

    public bool EnemyIsDead()
    {
        if (enemyHP < 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DamageTextAppear(int damageValue)
    {
        GameObject damage = Instantiate(damageTextPrefab, clickDamageTextContainer.transform);
        damage.GetComponent<Text>().text = "" + damageValue;
    }
}
