using UnityEngine;

public class BallCollisionHandler : MonoBehaviour
{
    [Header("Aceleración por Rebote")]
    [SerializeField] private float speedMultiplier = 1.05f;
    [SerializeField] private float maxSpeed = 25f;

    private BallController ballController;
    private BallDeathHandler deathHandler;

    private void Awake()
    {
        ballController = GetComponent<BallController>();
        deathHandler = GetComponent<BallDeathHandler>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ApplyDamage(1);
            }

            if (deathHandler != null)
            {
                deathHandler.TriggerDeath();
            }

            return;
        }

        BrickData brick = collision.gameObject.GetComponent<BrickData>();

        if (brick != null && brick.data != null)
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.AddScore(brick.data.points);
                BrickManager.Instance.DecrementBrickCount();
            }
            

            Destroy(collision.gameObject);
        }
        else
        {
            // Le delegamos el cambio de velocidad al BallController
            if (ballController != null)
            {
                ballController.AccelerateBall(speedMultiplier, maxSpeed);
            }
        }
    }
}
