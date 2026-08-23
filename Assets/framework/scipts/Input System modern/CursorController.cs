using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private bool hideOnStart = true;

    private void OnEnable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnGameStateChanged += HandleGameState;
        }
    }

    private void OnDisable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnGameStateChanged -= HandleGameState;
        }
    }

    private void HandleGameState(GameState state)
    {

        if (state == GameState.Playing && hideOnStart)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}