using UnityEngine;
using TMPro;

public class MainMenuBestFloorUI : MonoBehaviour
{
    [Header("최고 층수 표시 텍스트")]
    public TextMeshProUGUI bestFloorText;

    private void Start()
    {
        ShowBestFloor();
    }

    private void ShowBestFloor()
    {
        if (bestFloorText == null)
        {
            Debug.LogWarning("Best Floor Text가 등록되지 않았습니다.");
            return;
        }

        int bestFloor = BestFloorJS.GetBestFloor();

        if (bestFloor <= 0)
        {
            bestFloorText.text = "Best Score : Empty";
        }
        else
        {
            bestFloorText.text = "Best Score : F-" + bestFloor;
        }
    }
}