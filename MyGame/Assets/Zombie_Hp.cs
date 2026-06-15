using UnityEngine;
using System.Collections;

public class ZombieHealth : MonoBehaviour
{
    [Header("기본 능력치")]
    public int baseHealth = 5;
    public int baseAttackPower = 1;

    [Header("층수 강화 설정")]
    public int floorsPerPowerUp = 8;
    public int healthIncreasePerPowerUp = 1;
    public int attackIncreasePerPowerUp = 1;

    [Header("현재 능력치")]
    public int maxHealth;
    public int currentHealth;
    public int attackPower;

    [Header("피격 색상")]
    public Color hitColor = Color.red;
    public float blinkTime = 0.1f;

    [Header("사망 시 활성화할 상자")]
    public GameObject chestToActivateOnDeath;
    public bool hideChestOnStart = true;

    private SpriteRenderer sr;
    private Color originalColor;
    private Coroutine blinkCoroutine;

    private bool isDead = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            originalColor = sr.color;
        }

        if (hideChestOnStart && chestToActivateOnDeath != null)
        {
            chestToActivateOnDeath.SetActive(false);
        }

        ApplyFloorScaling();
    }

    private void ApplyFloorScaling()
    {
        int currentFloor = LevelStartText.CurrentFloor;

        int powerUpCount = currentFloor / floorsPerPowerUp;

        maxHealth = baseHealth + (healthIncreasePerPowerUp * powerUpCount);
        currentHealth = maxHealth;

        attackPower = baseAttackPower + (attackIncreasePerPowerUp * powerUpCount);

        Debug.Log(gameObject.name +
                  " 생성 / 현재 층: F-" + currentFloor +
                  " / 체력: " + maxHealth +
                  " / 공격력: " + attackPower);
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        Debug.Log(gameObject.name + " 피격! 남은 체력: " + currentHealth + " / " + maxHealth);

        if (sr != null)
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }

            blinkCoroutine = StartCoroutine(HitBlink());

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayZombieHit();
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitBlink()
    {
        sr.color = hitColor;

        yield return new WaitForSeconds(blinkTime);

        if (sr != null)
        {
            sr.color = originalColor;
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        ActivateRewardChest();

        Destroy(gameObject);
    }

    private void ActivateRewardChest()
    {
        if (chestToActivateOnDeath != null)
        {
            chestToActivateOnDeath.SetActive(true);
            Debug.Log("보스 보상 상자 활성화!");
        }
    }
}