using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageManager : MonoBehaviour
{
    public static DamageManager instance;
    public World world;
    public int clickDamage;
    public int autoDamage;
    public int enemyHP;
    public int damageTaked;
    public Image hPBar;
    public Text hPText;
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

        hPText.text = AbreviationManager.AbbreviateNumber(enemyHP);
        clickDamageInfo.text = AbreviationManager.AbbreviateNumber(clickDamage);
        autoDamageInfo.text = AbreviationManager.AbbreviateNumber(autoDamage);
    }

    public void ClickDamage()
    {
        EnemyIsDead();
        if (GameManager.instance != null && !EnemyIsDead())
        {
            if (SFXManager.instance.isActiveAndEnabled)
                SFXManager.instance.PlaySFX(Clip.Shoot);
            enemyHP -= clickDamage;
            damageTaked += clickDamage;
            Enemy.instance.anim.Play("Enemy_damage");
            HPBar();
            HPText();
            DamageTextAppear(clickDamage);
        }
        if (EnemyIsDead())
        {
            //SFXManager.instance.PlaySFX(Clip.Death);
            GameManager.instance.EarningCoins();
            enemyHP = GameManager.instance.currentEnemyHP;
            hPText.text = AbreviationManager.AbbreviateNumber(enemyHP);
            damageTaked = 0;
            hPBar.fillAmount = 0;
            GameManager.instance.GenerateNewEnemy();
            GameManager.instance.EnemyKiled();
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
                if (SFXManager.instance.isActiveAndEnabled)
                    SFXManager.instance.PlaySFX(Clip.Shoot);
                enemyHP -= autoDamage;
                damageTaked += autoDamage;
                Enemy.instance.anim.Play("Enemy_damage");
                HPBar();
                HPText();
                DamageTextAppear(autoDamage);
            }
            if (EnemyIsDead())
            {
                //SFXManager.instance.PlaySFX(Clip.Death);
                GameManager.instance.EarningCoins();
                enemyHP = GameManager.instance.currentEnemyHP;
                hPText.text = AbreviationManager.AbbreviateNumber(enemyHP);
                damageTaked = 0;
                hPBar.fillAmount = 0;
                GameManager.instance.GenerateNewEnemy();
                GameManager.instance.EnemyKiled();
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
            hPText.text = hPText.text = AbreviationManager.AbbreviateNumber(enemyHP); ;
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
        damage.GetComponent<Text>().text = AbreviationManager.AbbreviateNumber(damageValue);
    }
}
