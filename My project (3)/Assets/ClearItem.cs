using UnityEngine;
using TMPro;

public class ClearItem : MonoBehaviour
{
    public TMP_Text stageClearText;

    public float playerScaleMultiplier = 20f;

    private bool isCleared = false;

    void Start()
    {
        if (stageClearText != null)
        {
            stageClearText.text = "STAGE Clear!";
            stageClearText.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isCleared)
            return;

        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            isCleared = true;

            CharacterController controller = player.GetComponent<CharacterController>();

            // CharacterController 붙어있으면 잠깐 껐다가 크기 변경
            if (controller != null)
                controller.enabled = false;

            player.transform.localScale *= playerScaleMultiplier;

            if (controller != null)
                controller.enabled = true;

            if (stageClearText != null)
            {
                stageClearText.text = "STAGE Clear!";
                stageClearText.gameObject.SetActive(true);
            }

            Destroy(gameObject);
        }
    }
}