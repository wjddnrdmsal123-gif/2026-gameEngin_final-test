using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("도달 층수 표시 텍스트")]
    public TextMeshProUGUI floorText;

    [Header("다시 시작할 씬 이름")]
    public string firstLevelSceneName = "First_Level";

    [Header("메인 화면 씬 이름")]
    public string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        Time.timeScale = 1f;

        int reachedFloor = LevelStartText.CurrentFloor;

        // 최고 층수 저장
        BestFloorJS.SaveBestFloor(reachedFloor);

        ShowReachedFloor();
    }

    private void ShowReachedFloor()
    {
        if (floorText == null)
        {
            Debug.LogWarning("Floor Text가 등록되지 않았습니다.");
            return;
        }

        int reachedFloor = LevelStartText.CurrentFloor;
        int bestFloor = BestFloorJS.GetBestFloor();

        floorText.text =
            "도달 층수 : F-" + reachedFloor + "\n" +
            "최고 기록 : F-" + bestFloor;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;

        PlayerStats.ResetSavedStats();
        LevelStartText.ResetFloor();

        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;

        PlayerStats.ResetSavedStats();
        LevelStartText.ResetFloor();

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}