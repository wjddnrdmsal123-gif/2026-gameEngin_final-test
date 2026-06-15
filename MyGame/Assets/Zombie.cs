using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 1.5f;
    public float chaseSpeed = 2.5f;
    public float detectionRange = 5f;

    [Header("랜덤 이동 설정")]
    public float minChangeDirectionTime = 1f;
    public float maxChangeDirectionTime = 3f;

    [Header("방향별 걷기 스프라이트")]
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;

    [Header("애니메이션 속도")]
    public float frameTime = 0.15f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;

    private Vector2 moveDirection;

    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float animationTimer = 0f;

    private float directionTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        currentSprites = spriteDown;

        if (currentSprites != null && currentSprites.Length > 0)
        {
            sr.sprite = currentSprites[0];
        }

        ChooseRandomDirection();
    }

    private void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        UpdateMovementDirection();
        UpdateSpriteDirection();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        if (IsPlayerInRange())
        {
            currentSpeed = chaseSpeed;
        }

        rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
    }

    private void UpdateMovementDirection()
    {
        if (IsPlayerInRange())
        {
            Vector2 targetPosition = player.position;
            Vector2 currentPosition = transform.position;

            moveDirection = (targetPosition - currentPosition).normalized;
        }
        else
        {
            directionTimer -= Time.deltaTime;

            if (directionTimer <= 0f)
            {
                ChooseRandomDirection();
            }
        }
    }

    private bool IsPlayerInRange()
    {
        if (player == null)
            return false;

        float distance = Vector2.Distance(transform.position, player.position);

        return distance <= detectionRange;
    }

    private void ChooseRandomDirection()
    {
        int randomValue = Random.Range(0, 5);

        switch (randomValue)
        {
            case 0:
                moveDirection = Vector2.up;
                break;

            case 1:
                moveDirection = Vector2.down;
                break;

            case 2:
                moveDirection = Vector2.left;
                break;

            case 3:
                moveDirection = Vector2.right;
                break;

            case 4:
                moveDirection = Vector2.zero;
                break;
        }

        directionTimer = Random.Range(minChangeDirectionTime, maxChangeDirectionTime);
    }

    private void UpdateSpriteDirection()
    {
        if (moveDirection.sqrMagnitude <= 0.01f)
            return;

        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            if (moveDirection.x > 0)
            {
                ChangeSprites(spriteRight);
            }
            else
            {
                ChangeSprites(spriteLeft);
            }
        }
        else
        {
            if (moveDirection.y > 0)
            {
                ChangeSprites(spriteUp);
            }
            else
            {
                ChangeSprites(spriteDown);
            }
        }
    }

    private void UpdateAnimation()
    {
        if (currentSprites == null || currentSprites.Length == 0)
            return;

        if (moveDirection.sqrMagnitude <= 0.01f)
        {
            frameIndex = 0;
            sr.sprite = currentSprites[frameIndex];
            animationTimer = 0f;
            return;
        }

        animationTimer += Time.deltaTime;

        if (animationTimer >= frameTime)
        {
            animationTimer = 0f;
            frameIndex++;

            if (frameIndex >= currentSprites.Length)
            {
                frameIndex = 0;
            }

            sr.sprite = currentSprites[frameIndex];
        }
    }

    private void ChangeSprites(Sprite[] newSprites)
    {
        if (newSprites == null || newSprites.Length == 0)
            return;

        if (currentSprites == newSprites)
            return;

        currentSprites = newSprites;
        frameIndex = 0;
        animationTimer = 0f;
        sr.sprite = currentSprites[frameIndex];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}