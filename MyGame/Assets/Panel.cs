using UnityEngine;
using UnityEngine.EventSystems;

public class PanelClickClose : MonoBehaviour, IPointerClickHandler
{
    private GameObject currentChest;

    public void SetChest(GameObject chest)
    {
        currentChest = chest;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 패널 끄기
        gameObject.SetActive(false);

        // 상자 삭제
        if (currentChest != null)
        {
            Destroy(currentChest);
            currentChest = null;
        }

        // 게임 다시 진행
        Time.timeScale = 1f;
    }
}