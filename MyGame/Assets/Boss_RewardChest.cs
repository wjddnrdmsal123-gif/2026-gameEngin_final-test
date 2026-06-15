using UnityEngine;

public class BossRewardChest : MonoBehaviour
{
    [Header("획득 체력 증가량")]
    public int healthBonus = 3;

    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryCollect(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryCollect(other);
    }

    private void TryCollect(Collider2D other)
    {
        if (isCollected)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerStats playerStats = other.GetComponent<PlayerStats>();

        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats가 없습니다.");
            return;
        }

        isCollected = true;

        playerStats.AddMaxHealth(healthBonus);

        Debug.Log("보스 상자 획득! 최대 체력 +" + healthBonus);

        Destroy(gameObject);
    }
}