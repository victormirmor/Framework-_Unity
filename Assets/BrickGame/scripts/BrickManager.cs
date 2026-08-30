using UnityEngine;
using Framework;

namespace BrickGame
{
    public class BrickManager : MonoBehaviour
    {
        public static BrickManager Instance { get; private set; }

        [Header("Configuración de Capa")]
        [SerializeField] private string brickLayerName = "Brick";

        private int remainingBricks = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            CountBricksInScene();
        }

        private void CountBricksInScene()
        {
            int brickLayer = LayerMask.NameToLayer(brickLayerName);
            remainingBricks = 0;

            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (GameObject obj in allObjects)
            {
                if (obj.layer == brickLayer)
                {
                    remainingBricks++;
                }
            }

#if UNITY_EDITOR
            Debug.Log($"<color=cyan>[BrickManager DEBUG]</color> Ladrillos iniciales: <b>{remainingBricks}</b>");
#endif
        }

        public void DecrementBrickCount()
        {
            remainingBricks--;

#if UNITY_EDITOR
            Debug.Log($"<color=yellow>[BrickManager DEBUG]</color> Ladrillos restantes: <b>{remainingBricks}</b>");
#endif

            if (remainingBricks <= 0)
            {
                remainingBricks = 0;

                // Calificación explícita de Framework.GameState.Victory para evitar el conflicto de tipos
                if (GameController.Instance != null)
                {
                    GameController.Instance.SetGameState(Framework.GameState.Victory);
                }
            }
        }
    }
}