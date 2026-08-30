using UnityEngine;
using Framework;

namespace BrickGame
{
    public class TriggerCollisionManager : MonoBehaviour
    {
        [Header("Etiquetas y Capas")]
        [SerializeField] private string ballTag = "Ball";
        [SerializeField] private string floorLayerName = "Floor";
        [SerializeField] private string brickLayerName = "Brick";

        [Header("Referencias de Posicionamiento")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Vector3 ballRespawnOffset = new Vector3(0f, 0.75f, 0f);

        private GameObject ballObject;
        private BallController ballController;

        private void Awake()
        {
            if (playerTransform == null)
            {
                GameObject playerObj = GameObject.Find("Player_Bick");
                if (playerObj != null)
                {
                    playerTransform = playerObj.transform;
                }
            }
        }

        private void Start()
        {
            FindBallReference();
        }

        private void FindBallReference()
        {
            ballObject = GameObject.FindGameObjectWithTag(ballTag);
            if (ballObject != null)
            {
                ballController = ballObject.GetComponent<BallController>();
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"<color=red>[TriggerCollisionManager DEBUG]</color> No se encontró ningún objeto con el Tag '{ballTag}' en la escena.");
#endif
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(ballTag)) return;

            int otherLayer = other.gameObject.layer;

            // Detección del Suelo / Muerte
            if (otherLayer == LayerMask.NameToLayer(floorLayerName))
            {
#if UNITY_EDITOR
                Debug.Log($"<color=yellow>[TriggerCollisionManager DEBUG]</color> ¡SUELO DETECTADO! Objeto: <b>{other.gameObject.name}</b> en Layer: <b>{floorLayerName}</b>");
#endif
                if (GameController.Instance != null)
                {
                    GameController.Instance.LoseLife(1);
                }

                Audio_control.AudioControl.StartClip(SoundType.Ball);
                ResetAndAttachBallToPlayer(other.gameObject);
                return;
            }

            // Detección de Ladrillos
            if (otherLayer == LayerMask.NameToLayer(brickLayerName))
            {
                BrickData brick = other.GetComponent<BrickData>();

                if (brick != null && brick.data != null)
                {
                    if (GameController.Instance != null)
                    {
                        GameController.Instance.AddScore(brick.data.points);
                    }

                    if (BrickManager.Instance != null)
                    {
                        BrickManager.Instance.DecrementBrickCount();
                    }

                    Audio_control.AudioControl.StartClip(SoundType.Enemy);
                    Destroy(other.gameObject);
                }
            }
        }

        private void ResetAndAttachBallToPlayer(GameObject ball)
        {
            if (ballController == null)
            {
                ballController = ball.GetComponent<BallController>();
            }

            if (ballController != null)
            {
                ballController.StopBall();
                ballController.ResetToPaddle();
            }

            if (playerTransform != null)
            {
                ball.transform.SetParent(playerTransform);
                ball.transform.localPosition = ballRespawnOffset;

#if UNITY_EDITOR
                Debug.Log($"<color=green>[TriggerCollisionManager DEBUG]</color> Pelota reubicada exitosamente sobre: <b>{playerTransform.name}</b>");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("<color=red>[TriggerCollisionManager DEBUG]</color> No se pudo reubicar la pelota: 'playerTransform' es nulo.");
#endif
            }
        }
    }
}
