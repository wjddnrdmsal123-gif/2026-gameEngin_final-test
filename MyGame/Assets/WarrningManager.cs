using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WarningIntro : MonoBehaviour
{
    [Header("경고 이미지 오브젝트")]
    public GameObject warningImageObject;

    [Header("표시 시간")]
    public float warningDuration = 2f;

    [Header("깜빡임 속도")]
    public float blinkSpeed = 4f;

    [Header("이미지 크기")]
    public Vector2 warningSize = new Vector2(900f, 300f);

    private CanvasGroup canvasGroup;

    private void Start()
    {
        StartCoroutine(ShowWarning());
    }

    private IEnumerator ShowWarning()
    {
        // 한 프레임 기다렸다가 실행
        yield return null;

        if (warningImageObject == null)
        {
            Debug.LogWarning("Warning Image Object가 등록되지 않았습니다.");
            yield break;
        }

        Time.timeScale = 0f;

        // 강제로 켜기
        warningImageObject.SetActive(true);

        // UI에서 가장 앞으로 보내기
        warningImageObject.transform.SetAsLastSibling();

        RectTransform rect = warningImageObject.GetComponent<RectTransform>();

        if (rect != null)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = warningSize;
            rect.localScale = Vector3.one;
        }

        Image image = warningImageObject.GetComponent<Image>();

        if (image != null)
        {
            image.enabled = true;
            image.raycastTarget = false;

            Color color = image.color;
            color.a = 1f;
            image.color = color;
        }
        else
        {
            Debug.LogWarning("Warning Image Object에 Image 컴포넌트가 없습니다.");
        }

        canvasGroup = warningImageObject.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = warningImageObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float timer = 0f;

        while (timer < warningDuration)
        {
            timer += Time.unscaledDeltaTime;

            float alpha = Mathf.PingPong(timer * blinkSpeed, 1f);
            canvasGroup.alpha = Mathf.Lerp(0.25f, 1f, alpha);

            yield return null;
        }

        canvasGroup.alpha = 0f;
        warningImageObject.SetActive(false);

        Time.timeScale = 1f;
    }
}