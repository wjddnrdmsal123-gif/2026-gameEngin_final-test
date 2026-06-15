using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MovingPlatformY : MonoBehaviour
{
    public float moveDistance = 20f; // 위아래 이동 거리
    public float moveSpeed = 6f;     // 속도. 기존보다 빠르게

    public Vector3 PlatformVelocity { get; private set; }

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 lastPosition;
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.up * moveDistance;
        lastPosition = transform.position;
    }

    void Update()
    {
        lastPosition = transform.position;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        PlatformVelocity = (transform.position - lastPosition) / Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            direction *= -1;
            targetPosition = startPosition + Vector3.up * moveDistance * direction;
        }
    }
}