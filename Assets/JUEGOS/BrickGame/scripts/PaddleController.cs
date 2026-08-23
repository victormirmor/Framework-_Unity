using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PaddleController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float speed = 12f;

    private Rigidbody2D rb;
    private float horizontalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Si no estamos en estado de juego activo, frenamos la velocidad
        if (LevelManager.Instance != null && LevelManager.Instance.CurrentState != GameState.Playing)
        {
            horizontalInput = 0f;
            return;
        }

        // Lectura de entrada
        horizontalInput = (InputDataMap.Instance != null) ? InputDataMap.Instance.horizontal : Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        // Aplicamos velocidad física directa en X (igual que la bola)
        rb.velocity = new Vector2(horizontalInput * speed, 0f);
    }
}