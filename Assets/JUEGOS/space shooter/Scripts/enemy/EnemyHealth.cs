using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Componente contenedor para la vitalidad del enemigo en escena.
    /// Permite asociar la configuración (EnemyDataSO) y procesar el daño 
    /// provocado por los disparos del jugador.
    /// </summary>
    public class EnemyHealth : MonoBehaviour
    {
        [Header("Configuración de Datos")]
        public EnemyDataSO data;

        [Header("Efectos Visuales")]
        public GameObject explosionEffect;

        private int currentHealth;

        private void Start()
        {
            // Inicializar la salud a partir del ScriptableObject asignado
            if (data != null)
            {
                currentHealth = data.maxHealth;
            }
            else
            {
                currentHealth = 1;
            }
        }

        /// <summary>
        /// Procesa el daño recibido por disparos del jugador.
        /// </summary>
        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Gestiona la muerte del enemigo, otorgando puntaje al LevelManager
        /// e instanciando los efectos visuales.
        /// </summary>
        public void Die()
        {
            // Instanciar explosión
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, transform.rotation);
            }

            // Otorgar puntos al jugador mediante el LevelManager del framework
            if (LevelManager.Instance != null && data != null)
            {
                LevelManager.Instance.AddScore(data.points);
            }

            // Destruir el GameObject del enemigo
            Destroy(gameObject);
        }
    }
}