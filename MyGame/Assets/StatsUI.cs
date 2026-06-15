using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [Header("플레이어 스탯")]
    public PlayerStats playerStats;

    [Header("능력치 표시 텍스트")]
    public TextMeshProUGUI statsText;

    [Header("갱신 간격")]
    public float updateInterval = 0.1f;

    private float timer = 0f;

    private void Start()
    {
        FindPlayerStats();
        UpdateStatsText();
    }

    private void Update()
    {
        // 씬 이동 후 PlayerStats를 다시 찾아야 할 수도 있음
        if (playerStats == null)
        {
            FindPlayerStats();
        }

        timer += Time.unscaledDeltaTime;

        if (timer >= updateInterval)
        {
            timer = 0f;
            UpdateStatsText();
        }
    }

    private void FindPlayerStats()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    private void UpdateStatsText()
    {
        if (statsText == null)
            return;

        if (playerStats == null)
        {
            statsText.text = "스탯 없음";
            return;
        }

        statsText.text =
            "Speed : " + playerStats.moveSpeed.ToString("0.0") + "\n" +
            "Attack : " + playerStats.attackPower + "\n" +
            "Attack Time : " + playerStats.attackSpeed.ToString("0.0") + "\n" +
            "HP : " + playerStats.currentHealth + " / " + playerStats.maxHealth;
    }
}