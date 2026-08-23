using UnityEngine;

public class BallDeathHandler : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform paddleTransform;

    [Header("Configuración de Respawn")]
    [SerializeField] private float respawnDelay = 2.0f;
    [SerializeField] private Vector3 respawnOffset = new Vector3(0f, 0.5f, 0f);

    private BallController ballController;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        ballController = GetComponent<BallController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TriggerDeath()
    {
        if (ballController != null)
        {
            ballController.StopBall();
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        Invoke(nameof(RespawnBall), respawnDelay);
    }

    private void RespawnBall()
    {
        if (paddleTransform != null)
        {
            transform.SetParent(paddleTransform);
            transform.localPosition = respawnOffset;
        }

        // Reutilizamos la lógica del BallController
        if (ballController != null)
        {
            ballController.ResetToPaddle();
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }
}