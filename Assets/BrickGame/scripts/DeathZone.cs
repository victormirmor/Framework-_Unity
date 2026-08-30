using UnityEngine;
using Framework;

namespace BrickGame
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            BallController ball = collision.GetComponent<BallController>();
            if (ball != null)
            {
                if (GameController.Instance != null)
                {
                    GameController.Instance.LoseLife(1);
                }
            }
        }
    }
}