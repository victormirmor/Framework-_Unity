using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour 
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject pausePanel;

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
        if (pausePanel != null)
        {
            pausePanel.SetActive(state == GameState.Paused);
        }
    }

    // Métodos vinculables a los OnClick() de los botones del Panel de Pausa
    public void ResumeGame()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.SetGameState(GameState.Playing);
        }
    }

    public void ReturnToMainMenu(string mainMenuSceneName)
    {
        Time.timeScale = 1f;
        
        // Si existe una instancia persistente de GameManager, la destruimos al salir
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}