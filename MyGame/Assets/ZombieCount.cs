using UnityEngine;
using TMPro;

public class ZombieCountUI : MonoBehaviour
{
    [Header("좀비 수 표시 텍스트")]
    public TextMeshProUGUI zombieCountText;

    [Header("갱신 간격")]
    public float updateInterval = 0.2f;

    private float timer = 0f;

    private void Start()
    {
        UpdateZombieCount();
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;

        if (timer >= updateInterval)
        {
            timer = 0f;
            UpdateZombieCount();
        }
    }

    private void UpdateZombieCount()
    {
        if (zombieCountText == null)
            return;

        ZombieHealth[] zombies = FindObjectsOfType<ZombieHealth>();

        int count = 0;

        for (int i = 0; i < zombies.Length; i++)
        {
            if (zombies[i] != null && zombies[i].gameObject.activeInHierarchy)
            {
                count++;
            }
        }

        zombieCountText.text = "Zombies : " + count;
    }
}