using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("좀비 프리팹")]
    public GameObject zombiePrefab;

    [Header("기본 좀비 수")]
    public int baseZombieCount = 2;

    [Header("몇 층마다 좀비 추가")]
    public int floorsPerAddZombie = 8;

    [Header("플레이어와 최소 거리")]
    public float minDistanceFromPlayer = 4f;

    [Header("스폰 범위")]
    public Vector2 spawnAreaMin = new Vector2(-8f, -4f);
    public Vector2 spawnAreaMax = new Vector2(8f, 4f);

    [Header("스폰 위치 검사")]
    public float checkRadius = 0.4f;
    public LayerMask blockedLayers;

    [Header("시도 횟수")]
    public int maxSpawnAttempts = 100;

    private Transform player;

    private void Start()
    {
        FindPlayer();
        SpawnZombies();
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void SpawnZombies()
    {
        if (zombiePrefab == null)
        {
            Debug.LogWarning("Zombie Prefab이 등록되지 않았습니다.");
            return;
        }

        if (player == null)
        {
            FindPlayer();
        }

        int currentFloor = LevelStartText.CurrentFloor;

        int addCount = currentFloor / floorsPerAddZombie;
        int zombieCount = baseZombieCount + addCount;

        Debug.Log("현재 층: F-" + currentFloor + " / 생성 좀비 수: " + zombieCount);

        for (int i = 0; i < zombieCount; i++)
        {
            Vector2 spawnPos = GetRandomSpawnPosition();

            Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

            Vector2 randomPos = new Vector2(randomX, randomY);

            if (IsValidSpawnPosition(randomPos))
            {
                return randomPos;
            }
        }

        Debug.LogWarning("적절한 스폰 위치를 찾지 못했습니다. 임시 위치에 생성합니다.");
        return transform.position;
    }

    private bool IsValidSpawnPosition(Vector2 position)
    {
        if (player != null)
        {
            float distance = Vector2.Distance(position, player.position);

            if (distance < minDistanceFromPlayer)
            {
                return false;
            }
        }

        if (blockedLayers.value != 0)
        {
            Collider2D hit = Physics2D.OverlapCircle(position, checkRadius, blockedLayers);

            if (hit != null)
            {
                return false;
            }
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 center = new Vector3(
            (spawnAreaMin.x + spawnAreaMax.x) / 2f,
            (spawnAreaMin.y + spawnAreaMax.y) / 2f,
            0f
        );

        Vector3 size = new Vector3(
            Mathf.Abs(spawnAreaMax.x - spawnAreaMin.x),
            Mathf.Abs(spawnAreaMax.y - spawnAreaMin.y),
            0f
        );

        Gizmos.DrawWireCube(center, size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}