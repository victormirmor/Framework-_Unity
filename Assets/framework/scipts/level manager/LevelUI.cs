using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    [Header("Paneles de UI")]
    [SerializeField] private GameObject pausePanel;

    [Header("Elementos de Texto (TextMeshPro)")]
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;

    [Header("Configuración de Avisos")]
    [SerializeField] private float lifeMessageDuration = 2.0f;

    private void OnEnable()
    {
        ClearGameOverText();

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnGameStateChanged += HandleStateUI;
            LevelManager.Instance.OnScoreChanged += UpdateScoreUI;
            LevelManager.Instance.OnLivesChanged += UpdateLivesUI;

            // Sincronización inmediata al habilitarse la vista
            UpdateScoreUI(LevelManager.Instance.CurrentScore);
            UpdateLivesUI(LevelManager.Instance.CurrentLives);
        }
    }

    private void OnDisable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnGameStateChanged -= HandleStateUI;
            LevelManager.Instance.OnScoreChanged -= UpdateScoreUI;
            LevelManager.Instance.OnLivesChanged -= UpdateLivesUI;
        }
    }

    private void HandleStateUI(GameState state)
    {
        if (pausePanel != null) 
        {
            pausePanel.SetActive(state == GameState.Paused);
        }

        if (gameOverText != null)
        {
            CancelInvoke(nameof(ClearGameOverText));

            switch (state)
            {
                case GameState.GameOver:
                    gameOverText.text = "GAME OVER";
                    break;
                case GameState.Victory:
                    gameOverText.text = "¡VICTORIA!";
                    break;
                default:
                    ClearGameOverText();
                    break;
            }
        }
    }

    private void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"{newScore}";
        }
    }

    private void UpdateLivesUI(int currentLives)
    {
        if (livesText != null)
        {
            livesText.text = $"{currentLives}";
        }

        if (gameOverText != null)
        {
            CancelInvoke(nameof(ClearGameOverText));

            if (GameManager.Instance != null && GameManager.Instance.currentLanguage != null)
            {
                string livesPrefix = GameManager.Instance.currentLanguage.livesRemainingText;
                gameOverText.text = $"{livesPrefix}{currentLives}";
            }

            Invoke(nameof(ClearGameOverText), lifeMessageDuration);
        }
    }

    private void ClearGameOverText()
    {
        if (gameOverText != null)
        {
            gameOverText.text = "";
        }
    }
}