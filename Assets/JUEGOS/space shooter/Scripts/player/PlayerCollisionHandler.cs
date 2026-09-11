using UnityEngine;

namespace SpaceShooter
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        [Header("Efectos Visuales")]
        public GameObject playerExplosion;
        public GameObject defaultEnemyExplosion;

        [Header("Configuración de Penalizaciones")]
        public int penaltyPointsForBolt = 50;

        [Header("Modo Depuración / Debug")]
        public bool isInvincible = false;
        
        private PlayerDeathHandler deathHandler;

        private void Start()
        {
            deathHandler = GetComponent<PlayerDeathHandler>();
        }

        private void OnTriggerEnter(Collider other)
        {
            // 1. Validar que el juego se encuentre en ejecución activa
            if (LevelManager.Instance != null && LevelManager.Instance.CurrentState != GameState.Playing)
            {
                return;
            }

            // 2. Ignorar límites de escena o disparos propios
            if (other.CompareTag("Boundary") || other.CompareTag("PlayerBolt"))
            {
                return;
            }

            // 3. Evaluar colisiones con etiquetas dañinas
            if (other.CompareTag("Bolt_enemy"))
            {
                if (!isInvincible)
                {
                    ModifyScore(-penaltyPointsForBolt);
                }

                ProcessPlayerImpact(other.gameObject);
            }
            else if (other.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth != null && enemyHealth.data != null)
                {
                    ModifyScore(enemyHealth.data.points);
                }

                ProcessPlayerImpact(other.gameObject);
            }
        }

        private void ProcessPlayerImpact(GameObject collidedObject)
{
    // Ignorar si el jugador es invencible o si ya se encuentra en proceso de muerte
    if (isInvincible || (deathHandler != null && deathHandler.IsProcessingDeath))
    {
        if (defaultEnemyExplosion != null)
        {
            Instantiate(defaultEnemyExplosion, collidedObject.transform.position, collidedObject.transform.rotation);
        }

        Destroy(collidedObject);
        return;
    }

    // Desactivar el collider del objeto colisionado
    Collider otherCollider = collidedObject.GetComponent<Collider>();
    if (otherCollider != null)
    {
        otherCollider.enabled = false;
    }

    Destroy(collidedObject);

    if (LevelManager.Instance != null)
    {
        bool willSurvive = (LevelManager.Instance.CurrentLives - 1) > 0;

        LevelManager.Instance.ApplyDamage(1);

        if (willSurvive && deathHandler != null)
        {
            deathHandler.TriggerDeath();
        }
    }
}

        private void ModifyScore(int pointsAmount)
        {
            if (LevelManager.Instance != null && pointsAmount != 0)
            {
                LevelManager.Instance.AddScore(pointsAmount);
            }
        }
    }
}