using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public int scoreValue = 1;

    private bool isCollected = false;

    void OnTriggerEnter(Collider other)
    {
        if (isCollected)
            return;

        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            isCollected = true;

            ScoreManager.Instance.AddScore(scoreValue);

            Destroy(gameObject);
        }
    }
}