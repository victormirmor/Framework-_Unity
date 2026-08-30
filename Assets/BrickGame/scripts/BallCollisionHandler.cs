using UnityEngine;
using Framework;

namespace BrickGame
{
    public class BallCollisionHandler : MonoBehaviour
    {
        [Header("Aceleración por Rebote")]
        [SerializeField] private float speedMultiplier = 1.05f;
        [SerializeField] private float maxSpeed = 25f;

        private BallController ballController;
        private BallDeathHandler deathHandler;

        private void Awake()
        {
            ballController = GetComponent<BallController>();
            deathHandler = GetComponent<BallDeathHandler>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Colisión con el Suelo / Zona de Muerte
            if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                if (GameController.Instance != null)
                {
                    GameController.Instance.LoseLife(1);
                }

                Audio_control.AudioControl.StartClip(SoundType.Ball);

                // Forzamos la ejecución de la muerte y reposicionamiento
                if (deathHandler != null)
                {
                    deathHandler.TriggerDeath();
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogError("<color=red>[BallCollisionHandler DEBUG]</color> ¡Falta el componente BallDeathHandler en la pelota!");
#endif
                }

                return;
            }

            // Colisión con Ladrillos
            BrickData brick = collision.gameObject.GetComponent<BrickData>();

            if (brick != null && brick.data != null)
            {
                if (GameController.Instance != null)
                {
                    GameController.Instance.AddScore(brick.data.points);
                }

                if (BrickManager.Instance != null)
                {
                    BrickManager.Instance.DecrementBrickCount();
                }

                Audio_control.AudioControl.StartClip(SoundType.Enemy);
                Destroy(collision.gameObject);
            }
            else
            {
                if (ballController != null)
                {
                    ballController.AccelerateBall(speedMultiplier, maxSpeed);
                }
            }
        }
    }
}