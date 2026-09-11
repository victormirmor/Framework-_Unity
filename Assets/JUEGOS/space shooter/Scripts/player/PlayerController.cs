using UnityEngine;

namespace SpaceShooter
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        [SerializeField] [Range(1.0f, 20f)] private float speed = 12f;
        [SerializeField] [Range(0.1f, 2.0f)] private float rotate;

        [Header("Margen de Tolerancia")]
        [Tooltip("Margen extra fuera de pantalla antes de realizar el cambio de lado.")]
        [SerializeField] private float padding = 0.5f;

        private Rigidbody rb;
        private Camera mainCamera;
        private float horizontalInput, verticalInput;
        private float calculatedXBoundary;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            mainCamera = Camera.main;
        }

        private void Start()
        {
            CalculateScreenBounds();
        }

        /// <summary>
        /// Calcula automáticamente el límite del eje X según el ancho de la cámara.
        /// </summary>
        private void CalculateScreenBounds()
        {
            if (mainCamera != null)
            {
                // Obtener la distancia desde la cámara hasta la posición Z del jugador
                float distanceZ = Mathf.Abs(mainCamera.transform.position.y - transform.position.y);

                // Convertir el borde derecho del Viewport (x = 1) a coordenadas del mundo 3D
                Vector3 rightEdgeWorld = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, distanceZ));

                // Guardar el límite automático con el margen de compensación
                calculatedXBoundary = rightEdgeWorld.x + padding;
            }
            else
            {
                // Valor de respaldo en caso de no encontrar Camera.main
                calculatedXBoundary = 6f;
            }
        }

        private void Update()
        {
            // Si no estamos en estado de juego activo, frenamos la velocidad
            if (LevelManager.Instance != null && LevelManager.Instance.CurrentState != GameState.Playing)
            {
                horizontalInput = 0f;
                verticalInput = 0f;
                return;
            }

            // Lectura de entrada
            horizontalInput = (InputDataMap.Instance != null) ? InputDataMap.Instance.horizontal : Input.GetAxisRaw("Horizontal");
            verticalInput = (InputDataMap.Instance != null) ? InputDataMap.Instance.vertical : Input.GetAxisRaw("Vertical");
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector3(horizontalInput, 0f, verticalInput) * speed;
            rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -rotate);

            HandleScreenWrap();
        }

        /// <summary>
        /// Comprueba y reposiciona al jugador en el extremo opuesto cuando cruza el límite calculado.
        /// </summary>
        private void HandleScreenWrap()
        {
            Vector3 currentPosition = transform.position;

            if (currentPosition.x > calculatedXBoundary)
            {
                currentPosition.x = -calculatedXBoundary;
                transform.position = currentPosition;
            }
            else if (currentPosition.x < -calculatedXBoundary)
            {
                currentPosition.x = calculatedXBoundary;
                transform.position = currentPosition;
            }
        }
    }
}