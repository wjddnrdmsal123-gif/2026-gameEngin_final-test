using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;

    private int damage = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
        }
    }

    public void SetDirection(Vector2 direction, float speed)
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * speed;

            // 구버전 Unity에서 오류 나면 위 줄 대신 아래 줄 사용
            // rb.velocity = direction.normalized * speed;
        }
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        ZombieHealth zombieHealth = other.GetComponent<ZombieHealth>();

        if (zombieHealth != null)
        {
            zombieHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}