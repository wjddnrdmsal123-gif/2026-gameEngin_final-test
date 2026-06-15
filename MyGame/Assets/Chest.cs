using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("보상 패널")]
    public RewardPanel rewardPanel;

    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpened)
            return;

        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats == null)
            {
                Debug.LogWarning("플레이어에 PlayerStats가 없습니다.");
                return;
            }

            if (rewardPanel == null)
            {
                Debug.LogWarning("RewardPanel이 등록되지 않았습니다.");
                return;
            }

            rewardPanel.Open(playerStats, gameObject);
            isOpened = true;
        }
    }
}