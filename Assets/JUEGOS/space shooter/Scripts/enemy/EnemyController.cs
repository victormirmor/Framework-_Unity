using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Controla la lógica del enemigo respetando la estructura real de EnemyDataSO.
    /// Cancela disparos y movimientos cuando el juego no está en estado Playing.
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        [Header("Configuración de Datos")]
        public EnemyDataSO data;

        [Header("Configuración de Disparo Local")]
        public bool canShoot = false;
        public GameObject shotPrefab;
        public Transform shotSpawn;
        public float fireRate = 2f;

        private Rigidbody rb;
        private float nextFire;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();

            if (canShoot)
            {
                nextFire = Time.time + fireRate;
            }
        }

        private void Update()
        {
            // Detener disparos y movimiento si la partida no está en GameState.Playing
            if (LevelManager.Instance != null && LevelManager.Instance.CurrentState != GameState.Playing)
            {
                if (rb != null) rb.velocity = Vector3.zero;
                return;
            }

            // Ejecutar disparos locales si el enemigo está configurado para disparar
            if (canShoot && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Shoot();
            }
        }

        private void Shoot()
        {
            if (shotPrefab != null && shotSpawn != null)
            {
                Instantiate(shotPrefab, shotSpawn.position, shotSpawn.rotation);
            }
        }
    }
}