using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Rigidbody2D rb;
    private bool isBallActive = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ResetToPaddle();
    }

    private void Update()
    {
        bool isPlaying = LevelManager.Instance == null || LevelManager.Instance.CurrentState == GameState.Playing;
        bool firePressed = InputManager.GetButtonDown("Fire1");

        if (isPlaying && !isBallActive && firePressed) LaunchBall();
    }

    public void LaunchBall()
    {
        isBallActive = true;
        transform.SetParent(null);
        SetPhysicsActive(true);
        rb.velocity = new Vector2(0.5f, 1f).normalized * speed;
    }

    public void ResetToPaddle()
    {
        isBallActive = false;
        SetPhysicsActive(false);
    }

    public void StopBall()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void AccelerateBall(float multiplier, float maxSpeed)
    {
        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            float currentSpeed = rb.velocity.magnitude;
            float newSpeed = Mathf.Min(currentSpeed * multiplier, maxSpeed);
            rb.velocity = rb.velocity.normalized * newSpeed;
        }
    }

    private void SetPhysicsActive(bool active)
    {
        rb.bodyType = active ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        if (!active) StopBall();
    }
}