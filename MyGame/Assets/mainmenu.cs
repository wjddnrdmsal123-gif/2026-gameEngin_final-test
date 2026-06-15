using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    [Header("시작할 씬 이름")]
    public string firstSceneName = "First_Level";

    public void StartGame()
    {
        Time.timeScale = 1f;

        // 새 게임 시작 시 층수 / 스탯 초기화
        PlayerStats.ResetSavedStats();
        LevelStartText.ResetFloor();

        SceneManager.LoadScene(firstSceneName);
    }

    public void OpenOption()
    {
        Debug.Log("옵션 버튼 클릭");
    }

    public void ExitGame()
    {
        Debug.Log("게임 종료");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}