using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Victory
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Configuración de Transición")]
    [Tooltip("Nombre de la escena a la que se regresa tras un GameOver o al completar el juego")]
    [SerializeField] private string menuSceneName = "main_menu";
    
    [Tooltip("Segundos a esperar en pantalla de GameOver antes de cambiar de escena")]
    [SerializeField] private float gameOverDelay = 3.0f;

    public int CurrentScore { get; private set; }
    public int CurrentLives { get; private set; }
    public GameState CurrentState { get; private set; }

    // Eventos
    public event Action<GameState> OnGameStateChanged;
    public event Action<int> OnScoreChanged;
    public event Action<int> OnLivesChanged;

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
        CurrentScore = GameManager.Instance.globalScore;
        CurrentLives = GameManager.Instance.totalLives;

        OnScoreChanged?.Invoke(CurrentScore);
        OnLivesChanged?.Invoke(CurrentLives);

        // Inicia la partida directamente
        SetGameState(GameState.Playing);
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
        Time.timeScale = (CurrentState == GameState.Paused) ? 0f : 1f;

        Debug.Log($"[LevelManager] Cambio de Estado: ---> <b>{CurrentState}</b> | Time.timeScale: {Time.timeScale}");

        OnGameStateChanged?.Invoke(CurrentState);

        // Si entramos en GameOver, programamos el regreso a la escena del menú
        if (CurrentState == GameState.GameOver)
        {
            Invoke(nameof(ReturnToMenuScene), gameOverDelay);
        }
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
        {
            SetGameState(GameState.Paused);
        }
        else if (CurrentState == GameState.Paused)
        {
            SetGameState(GameState.Playing);
        }
    }

    public void OnResumeButtonPressed()
    {
        SetGameState(GameState.Playing);
    }

    public void AddScore(int points)
    {
        CurrentScore += points;
        OnScoreChanged?.Invoke(CurrentScore);
        GameManager.Instance.SetScore(GameManager.Instance.globalScore + points);
    }

    public void ApplyDamage(int damage)
    {
        CurrentLives -= damage;
        OnLivesChanged?.Invoke(CurrentLives);
        GameManager.Instance.SetLives(CurrentLives);

        if (CurrentLives <= 0)
        {
            SetGameState(GameState.GameOver);
        }
    }

    public void LevelComplete()
    {
        SetGameState(GameState.Victory);
        Invoke(nameof(LoadNextLevelOrMenu), 2.5f);
    }

    private void ReturnToMenuScene()
    {
        GameManager.Instance.currentLevelIndex = 1;

        if (!string.IsNullOrEmpty(menuSceneName))
        {
            SceneManager.LoadScene(menuSceneName);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    private void LoadNextLevelOrMenu()
    {
        int nextLevel = GameManager.Instance.currentLevelIndex + 1;
        int totalBuildScenes = SceneManager.sceneCountInBuildSettings;

        if (nextLevel < totalBuildScenes)
        {
            GameManager.Instance.currentLevelIndex = nextLevel;
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            ReturnToMenuScene();
        }
    }
}