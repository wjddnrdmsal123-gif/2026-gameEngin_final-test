using UnityEngine;
using System.Collections;

public class PlayerZombieDamage : MonoBehaviour
{
    [Header("기본 피격 데미지")]
    public int defaultDamage = 1;

    [Header("피격 쿨타임")]
    public float damageCooldown = 1f;

    [Header("씬 시작 무적 시간")]
    public float startInvincibleTime = 3f;

    [Header("피격 깜빡임")]
    public Color hitColor = Color.red;
    public float blinkInterval = 0.08f;
    public int blinkCount = 3;

    [Header("시작 무적 깜빡임")]
    public bool useStartInvincibleBlink = true;
    public Color invincibleColor = Color.cyan;
    public float invincibleBlinkInterval = 0.15f;

    private PlayerStats playerStats;
    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;

    private float nextDamageTime = 0f;
    private float invincibleEndTime = 0f;

    private Coroutine blinkCoroutine;
    private Coroutine invincibleCoroutine;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        originalColors = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors[i] = spriteRenderers[i].color;
        }
    }

    private void Start()
    {
        StartInvincible();
    }

    private void StartInvincible()
    {
        invincibleEndTime = Time.time + startInvincibleTime;

        if (useStartInvincibleBlink)
        {
            if (invincibleCoroutine != null)
            {
                StopCoroutine(invincibleCoroutine);
            }

            invincibleCoroutine = StartCoroutine(StartInvincibleBlink());
        }

        Debug.Log("씬 시작 무적 시작: " + startInvincibleTime + "초");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryTakeDamage(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryTakeDamage(other.gameObject);
    }

    private void TryTakeDamage(GameObject other)
    {
        // 씬 시작 무적 중이면 데미지 무시
        if (Time.time < invincibleEndTime)
            return;

        // 피격 쿨타임 중이면 데미지 무시
        if (Time.time < nextDamageTime)
            return;

        ZombieHealth zombieHealth = GetZombieHealth(other);

        if (zombieHealth == null)
            return;

        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats가 없습니다.");
            return;
        }

        int damage = zombieHealth.attackPower;

        if (damage <= 0)
        {
            damage = defaultDamage;
        }

        nextDamageTime = Time.time + damageCooldown;

        playerStats.TakeDamage(damage);

        Debug.Log("좀비에게 피격! 받은 데미지: " + damage);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerHit();
        }

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        blinkCoroutine = StartCoroutine(HitBlink());
    }

    private ZombieHealth GetZombieHealth(GameObject other)
    {
        ZombieHealth zombieHealth = other.GetComponent<ZombieHealth>();

        if (zombieHealth != null)
            return zombieHealth;

        zombieHealth = other.GetComponentInParent<ZombieHealth>();

        if (zombieHealth != null)
            return zombieHealth;

        zombieHealth = other.GetComponentInChildren<ZombieHealth>();

        if (zombieHealth != null)
            return zombieHealth;

        return null;
    }

    private IEnumerator HitBlink()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            SetColor(hitColor);
            yield return new WaitForSeconds(blinkInterval);

            RestoreColor();
            yield return new WaitForSeconds(blinkInterval);
        }

        RestoreColor();
    }

    private IEnumerator StartInvincibleBlink()
    {
        while (Time.time < invincibleEndTime)
        {
            SetColor(invincibleColor);
            yield return new WaitForSeconds(invincibleBlinkInterval);

            RestoreColor();
            yield return new WaitForSeconds(invincibleBlinkInterval);
        }

        RestoreColor();
    }

    private void SetColor(Color color)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
            {
                spriteRenderers[i].color = color;
            }
        }
    }

    private void RestoreColor()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
            {
                spriteRenderers[i].color = originalColors[i];
            }
        }
    }
}