using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Procesa el impacto de la bala del jugador contra enemigos y proyectiles.
    /// </summary>
    public class PlayerBoltCollision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // Ignorar límites de escena, al jugador o a otros disparos del propio jugador
            if (other.CompareTag("Boundary") || other.CompareTag("Player") || other.CompareTag("PlayerBolt"))
            {
                return;
            }

            // Impacto contra un enemigo
            if (other.CompareTag("Enemy"))
            {
                // 1. Notificar al gestor de salud/enemigos para procesar el daño
                if (EnemyHealthManager.Instance != null)
                {
                    EnemyHealthManager.Instance.ProcessEnemyHit(other.gameObject, 1);
                }

                // 2. Destruir la bala tras el impacto
                Destroy(gameObject);
            }
            // Impacto opcional contra un disparo enemigo (anulación de proyectiles)
            else if (other.CompareTag("Bolt_enemy"))
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}