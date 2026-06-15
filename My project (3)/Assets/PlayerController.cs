using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    public float platformCheckDistance = 0.4f;

    [Header("Game Over")]
    public TMP_Text gameOverText;
    public Vector3 startPosition = new Vector3(31.4f, 0.83f, -22.85f);
    public float resetDelay = 1f;
    public float gameOverCheckRadius = 1.2f;

    private CharacterController controller;
    private float yVelocity;
    private MovingPlatformX currentPlatform;
    private bool isGameOverRunning = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.text = "GAMEOVER";
            gameOverText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GAMEOVER 텍스트가 PlayerController에 연결 안 됨!");
        }
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        bool isGrounded = controller.isGrounded;

        currentPlatform = CheckPlatformBelow();

        Vector3 moveDir = Vector3.zero;

        if (keyboard.upArrowKey.isPressed)
            moveDir += Vector3.forward;

        if (keyboard.downArrowKey.isPressed)
            moveDir += Vector3.back;

        if (keyboard.leftArrowKey.isPressed)
            moveDir += Vector3.left;

        if (keyboard.rightArrowKey.isPressed)
            moveDir += Vector3.right;

        moveDir = moveDir.normalized;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * 100f * Time.deltaTime
            );
        }

        if (isGrounded && yVelocity < 0f)
        {
            yVelocity = -2f;
        }

        if (isGrounded && keyboard.spaceKey.wasPressedThisFrame)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = yVelocity;

        if (isGrounded && currentPlatform != null)
        {
            velocity += currentPlatform.PlatformVelocity;
        }

        controller.Move(velocity * Time.deltaTime);

        CheckGameOverZone();
    }

    private void CheckGameOverZone()
    {
        if (isGameOverRunning)
            return;

        float radius = Mathf.Max(gameOverCheckRadius, controller.height * 0.55f);

        Collider[] hits = Physics.OverlapSphere(
            controller.bounds.center,
            radius,
            ~0,
            QueryTriggerInteraction.Collide
        );

        foreach (Collider hit in hits)
        {
            GameOverZoneMarker marker = hit.GetComponentInParent<GameOverZoneMarker>();

            if (marker != null)
            {
                Debug.Log("게임오버 구역 감지됨: " + hit.name);
                StartCoroutine(GameOverRoutine());
                return;
            }
        }
    }

    private IEnumerator GameOverRoutine()
    {
        isGameOverRunning = true;

        if (gameOverText != null)
        {
            gameOverText.text = "GAMEOVER";
            gameOverText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(resetDelay);

        TeleportTo(startPosition);

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        isGameOverRunning = false;
    }

    public void TeleportTo(Vector3 position)
    {
        yVelocity = 0f;

        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;
    }

    private MovingPlatformX CheckPlatformBelow()
    {
        Vector3 origin = transform.position + controller.center;
        float rayLength = controller.height / 2f + platformCheckDistance;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayLength))
        {
            return hit.collider.GetComponentInParent<MovingPlatformX>();
        }

        return null;
    }
}