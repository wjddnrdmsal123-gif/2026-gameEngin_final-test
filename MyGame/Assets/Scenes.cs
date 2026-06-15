using UnityEngine;
using UnityEngine.SceneManagement;

public class EVLSceneMove : MonoBehaviour
{
    [Header("일반 씬")]
    [SerializeField] private string bonusSceneName = "Bonuse_Level";
    [SerializeField] private string emptySceneName = "Empty_Level";
    [SerializeField] private string monsterSceneName = "Monster_Level";

    [Header("중간보스 씬")]
    [SerializeField] private string midBossSceneName = "MidBoss_Level";

    [Header("몇 층마다 중간보스 씬으로 갈지")]
    [SerializeField] private int midBossEveryFloors = 20;

    private bool isLoading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLoading)
            return;

        if (other.CompareTag("Player"))
        {
            Chest[] chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);

            if (chests.Length > 0)
            {
                Debug.Log("아직 상자가 남아있어서 이동할 수 없습니다. 남은 상자 수: " + chests.Length);
                return;
            }

            ZombieHealth[] zombies = FindObjectsByType<ZombieHealth>(FindObjectsSortMode.None);

            if (zombies.Length > 0)
            {
                Debug.Log("아직 좀비가 남아있어서 이동할 수 없습니다. 남은 좀비 수: " + zombies.Length);
                return;
            }

            isLoading = true;

            int nextFloor = LevelStartText.CurrentFloor + 1;

            // 20층마다 중간보스 씬으로 이동
            if (nextFloor % midBossEveryFloors == 0)
            {
                SceneManager.LoadScene(midBossSceneName);
                return;
            }

            string selectedScene = GetRandomScene();

            SceneManager.LoadScene(selectedScene);
        }
    }

    private string GetRandomScene()
    {
        int randomValue = Random.Range(0, 10);

        // 0이 나오면 Empty_Level
        // 즉 10분의 1 확률
        if (randomValue == 0)
        {
            return emptySceneName;
        }

        // 나머지 9칸은 Bonuse_Level / Monster_Level 반반
        int normalRandom = Random.Range(0, 2);

        if (normalRandom == 0)
        {
            return bonusSceneName;
        }
        else
        {
            return monsterSceneName;
        }
    }
}