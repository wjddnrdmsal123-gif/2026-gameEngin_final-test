using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("기본 능력치")]
    public float moveSpeed = 5f;
    public int attackPower = 1;

    [Header("공격 속도")]
    public float attackSpeed = 1f;

    [Header("체력")]
    public int maxHealth = 5;
    public int currentHealth = 5;

    private static bool hasSavedStats = false;

    private static float savedMoveSpeed;
    private static int savedAttackPower;
    private static float savedAttackSpeed;
    private static int savedMaxHealth;
    private static int savedCurrentHealth;

    private bool isDead = false;

    private void Awake()
    {
        if (hasSavedStats == false)
        {
            SaveStats();
            hasSavedStats = true;
        }
        else
        {
            LoadStats();
        }
    }

    private void SaveStats()
    {
        savedMoveSpeed = moveSpeed;
        savedAttackPower = attackPower;
        savedAttackSpeed = attackSpeed;
        savedMaxHealth = maxHealth;
        savedCurrentHealth = currentHealth;
    }

    private void LoadStats()
    {
        moveSpeed = savedMoveSpeed;
        attackPower = savedAttackPower;
        attackSpeed = savedAttackSpeed;
        maxHealth = savedMaxHealth;
        currentHealth = savedCurrentHealth;
    }

    public void AddMoveSpeed(float amount)
    {
        moveSpeed += amount;
        SaveStats();
        Debug.Log("이속 증가! 현재 이속: " + moveSpeed);
    }

    public void AddAttackPower(int amount)
    {
        attackPower += amount;
        SaveStats();
        Debug.Log("공격력 증가! 현재 공격력: " + attackPower);
    }

    public void AddAttackSpeed(float amount)
    {
        attackSpeed += amount;
        SaveStats();
        Debug.Log("공속 증가! 현재 공속: " + attackSpeed);
    }

    public void AddMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        SaveStats();
        Debug.Log("체력 증가! 현재 체력: " + currentHealth + " / " + maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        SaveStats();

        Debug.Log("플레이어 피격! 현재 체력: " + currentHealth + " / " + maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Time.timeScale = 1f;

        SceneManager.LoadScene("GameOver");
    }

    public void Heal(int amount)
    {
        if (isDead)
            return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        SaveStats();

        Debug.Log("회복! 현재 체력: " + currentHealth + " / " + maxHealth);
    }

    public static void ResetSavedStats()
    {
        hasSavedStats = false;

        savedMoveSpeed = 0f;
        savedAttackPower = 0;
        savedAttackSpeed = 0f;
        savedMaxHealth = 0;
        savedCurrentHealth = 0;

        Debug.Log("저장된 스탯 초기화 완료");
    }
}