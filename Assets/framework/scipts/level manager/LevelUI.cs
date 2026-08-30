using UnityEngine;
using TMPro;

namespace Framework
{
    public class LevelUI : MonoBehaviour
    {
        [Header("Anotador Pasivo / Apuntador de Datos")]
        [SerializeField] private GameSessionDataSO sessionData;

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

            if (sessionData != null)
            {
#if UNITY_EDITOR
                Debug.Log($"<color=cyan>[LevelUI DEBUG]</color> Suscribiendo UI a eventos de: <b>{sessionData.name}</b>");
#endif
                sessionData.OnScoreChanged += UpdateScoreUI;
                sessionData.OnLivesChanged += UpdateLivesUI;

                UpdateScoreUI(sessionData.GlobalScore);
                UpdateLivesUI(sessionData.TotalLives);
            }

            if (GameController.Instance != null)
            {
                GameController.Instance.OnGameStateChanged += HandleStateUI;
            }
        }

        private void OnDisable()
        {
            if (sessionData != null)
            {
                sessionData.OnScoreChanged -= UpdateScoreUI;
                sessionData.OnLivesChanged -= UpdateLivesUI;
            }

            if (GameController.Instance != null)
            {
                GameController.Instance.OnGameStateChanged -= HandleStateUI;
            }
        }

        private void HandleStateUI(GameState state)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=purple>[LevelUI DEBUG]</color> Recibida notificación de cambio de estado de juego -> <b>{state}</b>");
#endif
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
#if UNITY_EDITOR
            Debug.Log($"<color=green>[LevelUI DEBUG]</color> Actualizando interfaz de Puntaje -> <b>{newScore}</b>");
#endif
            if (scoreText != null)
            {
                scoreText.text = $"{newScore}";
            }
        }

        private void UpdateLivesUI(int currentLives)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=orange>[LevelUI DEBUG]</color> Actualizando interfaz de Vidas -> <b>{currentLives}</b>");
#endif
            if (livesText != null)
            {
                livesText.text = $"{currentLives}";
            }

            if (gameOverText != null)
            {
                CancelInvoke(nameof(ClearGameOverText));

                if (sessionData != null && sessionData.currentLanguage != null)
                {
                    string livesPrefix = sessionData.currentLanguage.livesRemainingText;
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
}