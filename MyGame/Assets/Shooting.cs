using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [Header("발사체 프리팹")]
    public GameObject projectilePrefab;

    [Header("발사 위치 거리")]
    public float spawnDistance = 0.8f;

    [Header("발사체 속도")]
    public float projectileSpeed = 8f;

    [Header("기본 공격 쿨타임")]
    public float baseShootCooldown = 0.5f;

    private float lastShootTime = -999f;
    private Vector2 lastDirection = Vector2.down;

    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
        {
            lastDirection = Vector2.up;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
        {
            lastDirection = Vector2.down;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
        {
            lastDirection = Vector2.left;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
        {
            lastDirection = Vector2.right;
        }

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        float attackSpeed = 1f;

        if (playerStats != null)
        {
            attackSpeed = playerStats.attackSpeed;
        }

        float currentCooldown = baseShootCooldown / Mathf.Max(1f, attackSpeed);

        if (Time.time >= lastShootTime + currentCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("발사체 프리팹이 등록되지 않았습니다.");
            return;
        }

        Vector3 spawnPos = transform.position + (Vector3)(lastDirection * spawnDistance);

        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.SetDirection(lastDirection, projectileSpeed);

            int damage = 1;

            if (playerStats != null)
            {
                damage = playerStats.attackPower;
            }

            projectileScript.SetDamage(damage);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayShoot();
        }

    }
}