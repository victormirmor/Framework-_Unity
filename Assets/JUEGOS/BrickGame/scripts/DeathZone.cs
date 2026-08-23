using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BallController ball = collision.GetComponent<BallController>();
        if (ball != null)
        {
            // 1. Descontamos vida a través de LevelManager
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ApplyDamage(1);
            }

            // 2. Reposicionamos la bola sobre la paleta
            //ball.ResetBall();
        }
    }
}