using System.Collections;
using UnityEngine;

namespace SpaceShooter
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        [Header("Configuración de Respawn")]
        public float respawnDelay = 2.0f;
        
        [Header("Inmunidad Post-Respawn")]
        public float invincibilityDuration = 2.0f;

        [Header("Efectos Visuales")]
        public GameObject playerExplosion;

        private Vector3 initialPosition;
        private Quaternion initialRotation;

        private MeshRenderer meshRenderer;
        private Collider playerCollider;
        private PlayerCollisionHandler collisionHandler;

        // Bandera de control para evitar la ejecución simultánea de múltiples muertes
        public bool IsProcessingDeath { get; private set; } = false;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            playerCollider = GetComponent<Collider>();
            collisionHandler = GetComponent<PlayerCollisionHandler>();
        }

        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        public void TriggerDeath()
        {
            // Bloqueo inmediato: si ya estamos procesando una muerte, ignorar llamadas secundarias
            if (IsProcessingDeath) return;
            IsProcessingDeath = true;

            if (playerExplosion != null)
            {
                Instantiate(playerExplosion, transform.position, transform.rotation);
            }

            SetPlayerVisibilityAndCollider(false);

            if (LevelManager.Instance != null && LevelManager.Instance.CurrentLives > 0)
            {
                StartCoroutine(RespawnSequenceRoutine());
            }
        }

        private IEnumerator RespawnSequenceRoutine()
        {
#if UNITY_EDITOR
            Debug.Log($"<color=yellow>[Editor Debug Respawn]</color> Ocultando nave. Esperando {respawnDelay}s...");
#endif

            yield return new WaitForSeconds(respawnDelay);

            if (LevelManager.Instance != null && LevelManager.Instance.CurrentLives > 0 && LevelManager.Instance.CurrentState == GameState.Playing)
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;

                SetPlayerVisibilityAndCollider(true);

                if (collisionHandler != null)
                {
                    collisionHandler.isInvincible = true;
                }

#if UNITY_EDITOR
                Debug.Log($"<color=cyan>[Editor Debug Respawn]</color> Nave reaparecida | <b>isInvincible = TRUE</b> | Vidas Restantes: {LevelManager.Instance.CurrentLives}");
#endif

                yield return new WaitForSeconds(invincibilityDuration);

                if (collisionHandler != null)
                {
                    collisionHandler.isInvincible = false;
                }

#if UNITY_EDITOR
                Debug.Log("<color=orange>[Editor Debug Respawn]</color> Inmunidad finalizada | <b>isInvincible = FALSE</b>");
#endif
            }

            // Liberar el bloqueo para permitir futuras muertes cuando pierda la inmunidad
            IsProcessingDeath = false;
        }

        private void SetPlayerVisibilityAndCollider(bool isActive)
        {
            if (meshRenderer != null) meshRenderer.enabled = isActive;
            if (playerCollider != null) playerCollider.enabled = isActive;
        }
    }
}