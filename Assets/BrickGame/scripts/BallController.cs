using UnityEngine;
using Framework;

namespace BrickGame
{
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
            // Se especifica explícitamente Framework.GameState.Playing para evitar el conflicto entre enums
            if (GameController.Instance != null && GameController.Instance.CurrentState != Framework.GameState.Playing)
            {
                return;
            }

            if (!isBallActive && InputDataMap.Instance != null && InputDataMap.Instance.actionFire1)
            {
                LaunchBall();
            }
        }

        public void LaunchBall()
        {
            isBallActive = true;
            transform.SetParent(null);
            SetPhysicsActive(true);
            rb.velocity = new Vector2(0.5f, 1f).normalized * speed;

#if UNITY_EDITOR
            Debug.Log($"<color=green>[BallController DEBUG]</color> Pelota lanzada a velocidad: <b>{speed}</b>");
#endif
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

#if UNITY_EDITOR
                Debug.Log($"<color=yellow>[BallController DEBUG]</color> Pelota acelerada: {currentSpeed:F2} -> <b>{newSpeed:F2}</b> (Máx: {maxSpeed})");
#endif
            }
        }

        private void SetPhysicsActive(bool active)
        {
            rb.bodyType = active ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
            if (!active) StopBall();
        }
    }
}