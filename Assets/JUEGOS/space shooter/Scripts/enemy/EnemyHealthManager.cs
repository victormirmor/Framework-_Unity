using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Administra la salud, el procesamiento de impactos y la destrucción/limpieza
    /// por límites de los enemigos activos en la escena.
    /// </summary>
    public class EnemyHealthManager : MonoBehaviour
    {
        public static EnemyHealthManager Instance { get; private set; }

        [Header("Efectos Visuales Generales")]
        public GameObject defaultExplosion;

        private Dictionary<GameObject, int> enemyHealthMap = new Dictionary<GameObject, int>();
        private Dictionary<GameObject, EnemyDataSO> enemyDataMap = new Dictionary<GameObject, EnemyDataSO>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
               //Destroy(gameObject);
            }
        }

        public void RegisterEnemy(GameObject enemyObject, EnemyDataSO data)
        {
            if (enemyObject == null || enemyHealthMap.ContainsKey(enemyObject)) return;

            int initialHealth = (data != null) ? data.maxHealth : 1;

            enemyHealthMap.Add(enemyObject, initialHealth);
            enemyDataMap.Add(enemyObject, data);
        }

        public void ProcessEnemyHit(GameObject enemyObject, int damageAmount)
        {
            if (enemyObject == null || !enemyHealthMap.ContainsKey(enemyObject)) return;

            enemyHealthMap[enemyObject] -= damageAmount;

            if (enemyHealthMap[enemyObject] <= 0)
            {
                DestroyEnemy(enemyObject);
            }
        }

        /// <summary>
        /// Destrucción por combate (combina explosión, puntaje en LevelManager y remoción).
        /// </summary>
        public void DestroyEnemy(GameObject enemyObject)
        {
            if (enemyObject == null) return;

            if (defaultExplosion != null)
            {
                Instantiate(defaultExplosion, enemyObject.transform.position, enemyObject.transform.rotation);
            }

            if (enemyDataMap.TryGetValue(enemyObject, out EnemyDataSO data) && data != null)
            {
                if (LevelManager.Instance != null)
                {
                    LevelManager.Instance.AddScore(data.points);
                }
            }

            RemoveAndDestroy(enemyObject);
        }

        /// <summary>
        /// Destrucción silenciosa cuando la entidad colisiona con el Boundary o sale de escena.
        /// </summary>
        public void RemoveAndDestroy(GameObject enemyObject)
        {
            if (enemyObject == null) return;

            // Limpiar registro de diccionarios
            UnregisterEnemy(enemyObject);

            // Notificar remoción al controlador de oleadas
            if (SpawnController.Instance != null)
            {
                SpawnController.Instance.RemoveEnemyReference(enemyObject);
            }

            // Destruir objeto de la escena
            Destroy(enemyObject);
        }

        public void UnregisterEnemy(GameObject enemyObject)
        {
            if (enemyObject != null && enemyHealthMap.ContainsKey(enemyObject))
            {
                enemyHealthMap.Remove(enemyObject);
                enemyDataMap.Remove(enemyObject);
            }
        }
    }
}