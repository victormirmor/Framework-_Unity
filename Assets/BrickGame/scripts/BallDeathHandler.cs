using UnityEngine;

namespace BrickGame
{
    public class BallDeathHandler : MonoBehaviour
    {
        [Header("Referencias")]
        [SerializeField] private Transform paddleTransform;

        [Header("Configuración de Respawn")]
        [SerializeField] private float respawnDelay = 1.0f;
        [SerializeField] private Vector3 respawnOffset = new Vector3(0f, 0.75f, 0f);

        private BallController ballController;
        private SpriteRenderer spriteRenderer;
        private Collider2D ballCollider;

        private void Awake()
        {
            ballController = GetComponent<BallController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            ballCollider = GetComponent<Collider2D>();

            // Búsqueda de resguardo si no se asignó manualmente en el Inspector
            if (paddleTransform == null)
            {
                GameObject paddleObj = GameObject.Find("Player_Bick"); // Nombre exacto en tu Jerarquía
                if (paddleObj != null)
                {
                    paddleTransform = paddleObj.transform;
                }
            }
        }

        public void TriggerDeath()
        {
#if UNITY_EDITOR
            Debug.Log("<color=red>[BallDeathHandler DEBUG]</color> Pelota fuera de juego. Ocultando y programando respawn.");
#endif
            if (ballController != null)
            {
                ballController.StopBall();
            }

            if (spriteRenderer != null) spriteRenderer.enabled = false;
            if (ballCollider != null) ballCollider.enabled = false;

            Invoke(nameof(RespawnBall), respawnDelay);
        }

        private void RespawnBall()
        {
            if (paddleTransform != null)
            {
                transform.SetParent(paddleTransform);
                transform.localPosition = respawnOffset;
#if UNITY_EDITOR
                Debug.Log($"<color=green>[BallDeathHandler DEBUG]</color> Pelota reubicada en la paleta: {paddleTransform.name}");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("<color=red>[BallDeathHandler DEBUG]</color> No se encontró 'paddleTransform'. La pelota reapareció en la posición global.");
#endif
            }

            if (ballController != null)
            {
                ballController.ResetToPaddle();
            }

            if (spriteRenderer != null) spriteRenderer.enabled = true;
            if (ballCollider != null) ballCollider.enabled = true;
        }
    }
}