using UnityEngine;

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

        // Buscamos todos los GameObjects de la escena y contamos los que están en la Layer "Brick"
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == brickLayer)
            {
                remainingBricks++;
            }
        }

        Debug.Log($"[BrickManager] Ladrillos iniciales: {remainingBricks}");
    }

    // Método simple: restamos 1 al contador
    public void DecrementBrickCount()
    {
        remainingBricks--;
        Debug.Log($"[BrickManager] Ladrillos restantes: {remainingBricks}");

        if (remainingBricks <= 0)
        {
            remainingBricks = 0;

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.LevelComplete();
            }
        }
    }
}