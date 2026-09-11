using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceShooter
{
    /// <summary>
    /// Componente de Nivel 2 (Específico del Juego).
    /// Controla la instanciación de oleadas y gestiona la limpieza de enemigos
    /// consultando las reglas/estados expuestos por el LevelManager (Nivel 1).
    /// </summary>
    public class SpawnController : MonoBehaviour
    {
        public static SpawnController Instance { get; private set; }

        [Header("Configuración de Oleadas")]
        public GameObject[] hazards;
        public Vector3 spawnValues;
        public int hazardCount;
        public float spawnWait;
        public float startWait;
        public float waveWait;

        private List<GameObject> activeEnemies = new List<GameObject>();
        private EnemyHealthManager healthManager;
        private Coroutine spawnCoroutine;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            healthManager = GetComponent<EnemyHealthManager>();
        }

        private void Start()
        {
            spawnCoroutine = StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            yield return new WaitForSeconds(startWait);

            while (true)
            {
                // Consultar el estado expuesto por el LevelManager (Nivel 1)
                if (LevelManager.Instance != null && LevelManager.Instance.CurrentState == GameState.Playing)
                {
                    for (int i = 0; i < hazardCount; i++)
                    {
                        // Si el estado deja de ser Playing durante el bucle, se interrumpe la oleada
                        if (LevelManager.Instance.CurrentState != GameState.Playing)
                        {
                            break;
                        }

                        SpawnSingleHazard();
                        yield return new WaitForSeconds(spawnWait);
                    }
                }
                else if (LevelManager.Instance != null && LevelManager.Instance.CurrentState == GameState.GameOver)
                {
                    // Si el juego terminó, limpiar todas las entidades restantes y detener el bucle
                    ClearActiveEnemies();
                    yield break;
                }

                yield return new WaitForSeconds(waveWait);
            }
        }

        private void SpawnSingleHazard()
        {
            if (hazards == null || hazards.Length == 0) return;

            GameObject hazardPrefab = hazards[Random.Range(0, hazards.Length)];
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
            Quaternion spawnRotation = Quaternion.identity;

            GameObject spawnedHazard = Instantiate(hazardPrefab, spawnPosition, spawnRotation);

            activeEnemies.Add(spawnedHazard);

            EnemyData enemyData = spawnedHazard.GetComponent<EnemyData>();
            EnemyDataSO dataSO = (enemyData != null) ? enemyData.data : null;

            if (healthManager != null)
            {
                healthManager.RegisterEnemy(spawnedHazard, dataSO);
            }
        }

        /// <summary>
        /// Remueve la referencia de la lista local de oleadas.
        /// </summary>
        public void RemoveEnemyReference(GameObject enemyObject)
        {
            if (activeEnemies.Contains(enemyObject))
            {
                activeEnemies.Remove(enemyObject);
            }
        }

        /// <summary>
        /// Limpia de forma masiva todas las instancias de enemigos creadas por el spawner.
        /// </summary>
        public void ClearActiveEnemies()
        {
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                if (activeEnemies[i] != null)
                {
                    Destroy(activeEnemies[i]);
                }
            }
            activeEnemies.Clear();
        }

        private void OnDisable(){
        if (LevelManager.Instance != null){
        LevelManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
            StopAllCoroutines();
    }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

private void OnEnable()
{
    if (LevelManager.Instance != null)
    {
        LevelManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }
}


private void HandleGameStateChanged(GameState newState)
{
    if (newState == GameState.GameOver)
    {
        ClearActiveEnemies();
    }
}
    }
}