using UnityEngine;
using UnityEngine.UI;

public class MenuBackgroundAnimator : MonoBehaviour
{
    [Header("배경 이미지")]
    public Image targetImage;

    [Header("애니메이션 프레임")]
    public Sprite[] frames;

    [Header("프레임 전환 속도")]
    public float frameTime = 0.18f;

    [Header("불규칙 깜빡임")]
    public bool useFlicker = true;
    public float flickerStrength = 0.18f;
    public float flickerSpeed = 12f;

    [Header("지직거리는 흔들림")]
    public bool useGlitchShake = true;
    public float shakeAmount = 4f;
    public float shakeChance = 0.08f;

    private int frameIndex = 0;
    private float frameTimer = 0f;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 originalPosition;

    private void Awake()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        rectTransform = GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            originalPosition = rectTransform.anchoredPosition;
        }

        if (frames != null && frames.Length > 0 && targetImage != null)
        {
            targetImage.sprite = frames[0];
        }
    }

    private void Update()
    {
        AnimateFrames();
        FlickerEffect();
        GlitchShakeEffect();
    }

    private void AnimateFrames()
    {
        if (targetImage == null)
            return;

        if (frames == null || frames.Length == 0)
            return;

        frameTimer += Time.unscaledDeltaTime;

        if (frameTimer >= frameTime)
        {
            frameTimer = 0f;

            frameIndex++;

            if (frameIndex >= frames.Length)
            {
                frameIndex = 0;
            }

            targetImage.sprite = frames[frameIndex];
        }
    }

    private void FlickerEffect()
    {
        if (!useFlicker)
        {
            canvasGroup.alpha = 1f;
            return;
        }

        float flicker = Mathf.PerlinNoise(Time.unscaledTime * flickerSpeed, 0f);
        float alpha = 1f - (flicker * flickerStrength);

        canvasGroup.alpha = alpha;
    }

    private void GlitchShakeEffect()
    {
        if (!useGlitchShake)
            return;

        if (rectTransform == null)
            return;

        if (Random.value < shakeChance)
        {
            float randomX = Random.Range(-shakeAmount, shakeAmount);
            float randomY = Random.Range(-shakeAmount, shakeAmount);

            rectTransform.anchoredPosition = originalPosition + new Vector2(randomX, randomY);
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}