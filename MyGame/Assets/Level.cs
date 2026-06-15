using UnityEngine;
using TMPro;
using System.Collections;

public class LevelStartText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private float showTime = 2f;

    private static int floorNumber = 1;
    private static bool hasStartedBefore = false;

    public static int CurrentFloor
    {
        get { return floorNumber; }
    }

    private void Awake()
    {
        if (hasStartedBefore == false)
        {
            hasStartedBefore = true;
        }
        else
        {
            floorNumber++;
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(ShowLevelText());
    }

    private IEnumerator ShowLevelText()
    {
        if (levelText == null)
        {
            Debug.LogWarning("Level Text가 등록되지 않았습니다.");
            yield break;
        }

        levelText.gameObject.SetActive(true);
        levelText.text = "F-" + floorNumber;

        RectTransform rect = levelText.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;

        yield return new WaitForSecondsRealtime(showTime);

        levelText.gameObject.SetActive(false);
    }

    public static void ResetFloor()
    {
        floorNumber = 1;
        hasStartedBefore = false;
    }
}