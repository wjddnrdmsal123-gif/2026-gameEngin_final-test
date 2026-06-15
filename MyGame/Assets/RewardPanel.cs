using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum RewardType
{
    Boots,
    Syringe,
    Bow,
    Heart,
    Empty
}

[System.Serializable]
public class RewardOption
{
    public string rewardName;
    public Sprite icon;
    public RewardType rewardType;
    public float amount = 1f;
}

public class RewardPanel : MonoBehaviour, IPointerClickHandler
{
    [Header("중앙에 띄울 아이템 이미지")]
    public Image centerImage;

    [Header("아이콘 크기")]
    public Vector2 iconSize = new Vector2(350f, 350f);

    [Header("랜덤 보상 목록")]
    public RewardOption[] rewards = new RewardOption[4];

    private PlayerStats playerStats;
    private GameObject currentChest;

    private CanvasGroup canvasGroup;
    private bool isOpen = false;

    private void Awake()
    {
        SetupCanvasGroup();
        ForceHidePanel();
    }

    public void Open(PlayerStats stats, GameObject chest)
    {
        playerStats = stats;
        currentChest = chest;
        isOpen = true;

        // 비활성화되어 있어도 강제로 켜기
        gameObject.SetActive(true);

        SetupCanvasGroup();
        ForceShowPanel();

        Time.timeScale = 0f;

        ShowRandomReward();

        Debug.Log("보상 패널 열림 / Alpha: " + canvasGroup.alpha);
    }

    private void SetupCanvasGroup()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (centerImage != null)
        {
            centerImage.raycastTarget = false;
        }
    }

    private void ForceShowPanel()
    {
        gameObject.SetActive(true);

        // UI 맨 앞으로 올림
        transform.SetAsLastSibling();

        if (canvasGroup == null)
        {
            SetupCanvasGroup();
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // 패널 자체 Image도 강제로 보이게
        Image panelImage = GetComponent<Image>();

        if (panelImage != null)
        {
            panelImage.enabled = true;

            Color color = panelImage.color;
            color.a = 1f;
            panelImage.color = color;
        }
    }

    private void ForceHidePanel()
    {
        isOpen = false;

        if (canvasGroup == null)
        {
            SetupCanvasGroup();
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (centerImage != null)
        {
            centerImage.enabled = false;
        }
    }

    private void ShowRandomReward()
    {
        if (rewards == null || rewards.Length == 0)
        {
            Debug.LogWarning("보상 목록이 비어있습니다.");
            return;
        }

        int randomIndex = Random.Range(0, rewards.Length);
        RewardOption reward = rewards[randomIndex];

        if (centerImage != null)
        {
            centerImage.sprite = reward.icon;
            centerImage.enabled = reward.icon != null;
            centerImage.preserveAspect = true;
            centerImage.raycastTarget = false;

            RectTransform rect = centerImage.GetComponent<RectTransform>();

            if (rect != null)
            {
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = iconSize;
            }

            centerImage.transform.SetAsLastSibling();
        }

        ApplyReward(reward);
    }

    private void ApplyReward(RewardOption reward)
    {
        if (playerStats == null)
            return;

        switch (reward.rewardType)
        {
            case RewardType.Boots:
                playerStats.AddMoveSpeed(reward.amount);
                break;

            case RewardType.Syringe:
                playerStats.AddAttackPower(Mathf.RoundToInt(reward.amount));
                break;

            case RewardType.Bow:
                playerStats.AddAttackSpeed(reward.amount);
                break;

            case RewardType.Heart:
                playerStats.AddMaxHealth(Mathf.RoundToInt(reward.amount));
                break;

            case RewardType.Empty:
                Debug.Log("빈 보상입니다.");
                break;
        }

        Debug.Log("획득 보상: " + reward.rewardName);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayChest();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ForceHidePanel();

        if (currentChest != null)
        {
            Destroy(currentChest);
            currentChest = null;
        }

        Time.timeScale = 1f;
    }
}