using UnityEngine;
using System.Collections;

public class MidBossController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 1.0f;
    public float chaseSpeed = 1.8f;
    public float detectionRange = 7f;

    [Header("플레이어와 너무 가까울 때 멈추는 거리")]
    public float stopDistance = 0.3f;

    [Header("랜덤 이동 설정")]
    public float minChangeDirectionTime = 1.5f;
    public float maxChangeDirectionTime = 3.5f;

    [Header("돌진 패턴")]
    public float chargeInterval = 7f;
    public float chargeWarningTime = 0.7f;
    public float chargeSpeed = 8f;
    public float chargeDuration = 0.45f;

    [Header("돌진 후 정지 시간")]
    public float afterChargeStopTime = 1f;

    [Header("돌진 예고 색상")]
    public Color chargeWarningColor = Color.cyan;

    [Header("방향별 걷기 스프라이트")]
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;

    [Header("애니메이션 속도")]
    public float frameTime = 0.18f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;

    private Vector2 moveDirection;
    private Vector2 chargeDirection;

    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float animationTimer = 0f;

    private float directionTimer = 0f;

    private bool isPreparingCharge = false;
    private bool isCharging = false;
    private bool isAfterChargeStopping = false;

    private Color originalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }

        if (sr != null)
        {
            originalColor = sr.color;
        }

        FindPlayer();

        currentSprites = spriteDown;

        if (currentSprites != null && currentSprites.Length > 0)
        {
            sr.sprite = currentSprites[0];
        }

        ChooseRandomDirection();
    }

    private void Start()
    {
        StartCoroutine(ChargeRoutine());
    }

    private void Update()
    {
        if (player == null)
        {
            FindPlayer();
        }

        if (isPreparingCharge || isCharging || isAfterChargeStopping)
        {
            UpdateSpriteDirection();
            UpdateAnimation();
            return;
        }

        UpdateMovementDirection();
        UpdateSpriteDirection();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (rb == null)
            return;

        if (isPreparingCharge)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isCharging)
        {
            rb.linearVelocity = chargeDirection * chargeSpeed;
            return;
        }

        if (isAfterChargeStopping)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float currentSpeed = moveSpeed;

        if (IsPlayerInRange())
        {
            currentSpeed = chaseSpeed;
        }

        Vector2 velocity = moveDirection * currentSpeed;
        rb.linearVelocity = velocity;

        // 구버전 Unity에서 오류 나면 위 줄 대신 아래 줄 사용
        // rb.velocity = velocity;
    }

    private IEnumerator ChargeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(chargeInterval);

            if (player == null)
            {
                FindPlayer();
            }

            if (player == null)
                continue;

            StartChargeWarning();

            yield return StartCoroutine(ChargeWarningBlink());

            StartCharge();

            yield return new WaitForSeconds(chargeDuration);

            EndCharge();

            StartAfterChargeStop();

            yield return new WaitForSeconds(afterChargeStopTime);

            EndAfterChargeStop();
        }
    }

    private void StartChargeWarning()
    {
        isPreparingCharge = true;
        isCharging = false;
        isAfterChargeStopping = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        Vector2 targetPosition = player.position;
        Vector2 currentPosition = transform.position;

        chargeDirection = (targetPosition - currentPosition).normalized;

        if (chargeDirection.sqrMagnitude <= 0.01f)
        {
            chargeDirection = Vector2.down;
        }

        moveDirection = chargeDirection;
        UpdateSpriteDirection();
    }

    private IEnumerator ChargeWarningBlink()
    {
        float timer = 0f;
        float blinkInterval = 0.1f;
        bool blue = false;

        while (timer < chargeWarningTime)
        {
            blue = !blue;

            if (sr != null)
            {
                sr.color = blue ? chargeWarningColor : originalColor;
            }

            yield return new WaitForSeconds(blinkInterval);

            timer += blinkInterval;
        }

        if (sr != null)
        {
            sr.color = originalColor;
        }
    }

    private void StartCharge()
    {
        isPreparingCharge = false;
        isCharging = true;
        isAfterChargeStopping = false;

        moveDirection = chargeDirection;
        UpdateSpriteDirection();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBossCharge();
        }

    }

    private void EndCharge()
    {
        isCharging = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (sr != null)
        {
            sr.color = originalColor;
        }
    }

    private void StartAfterChargeStop()
    {
        isAfterChargeStopping = true;
        moveDirection = Vector2.zero;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        frameIndex = 0;

        if (currentSprites != null && currentSprites.Length > 0)
        {
            sr.sprite = currentSprites[frameIndex];
        }

        Debug.Log("프랑켄슈타인 돌진 후 1초 정지");
    }

    private void EndAfterChargeStop()
    {
        isAfterChargeStopping = false;

        ChooseRandomDirection();
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void UpdateMovementDirection()
    {
        if (IsPlayerInRange())
        {
            Vector2 targetPosition = player.position;
            Vector2 currentPosition = transform.position;

            float distance = Vector2.Distance(currentPosition, targetPosition);

            if (distance <= stopDistance)
            {
                moveDirection = Vector2.zero;
            }
            else
            {
                moveDirection = (targetPosition - currentPosition).normalized;
            }
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

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (sr != null)
        {
            sr.color = originalColor;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}