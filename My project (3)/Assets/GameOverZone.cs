using System.Collections;
using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    public GameObject gameOverText;

    public Vector3 startPosition = new Vector3(31.4f, 0.83f, -22.85f);
    public float resetDelay = 1f;

    private bool isGameOverRunning = false;

    void Start()
    {
        Debug.Log("GameOverZone 준비됨");

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameOver Text가 연결 안 됨!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("무언가 닿음: " + other.name);

        if (isGameOverRunning)
            return;

        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player == null)
        {
            Debug.LogWarning("닿긴 했는데 PlayerController를 못 찾음: " + other.name);
            return;
        }

        Debug.Log("플레이어 감지됨. 게임오버 실행!");

        StartCoroutine(GameOverRoutine(player));
    }

    IEnumerator GameOverRoutine(PlayerController player)
    {
        isGameOverRunning = true;

        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        yield return new WaitForSeconds(resetDelay);

        player.TeleportTo(startPosition);

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }

        isGameOverRunning = false;
    }
}