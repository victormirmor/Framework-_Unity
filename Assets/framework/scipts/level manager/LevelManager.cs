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

/// <summary>
/// Componente de Capa 1 (Framework General).
/// Reutilizable en cualquier juego. No conoce scripts específicos de Capa 2 o 3.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Configuración de Transición")]
    [Tooltip("Nombre de la escena a la que se regresa tras un GameOver o al completar el juego")]
    [SerializeField] private string menuSceneName = "main_menu";

    [Tooltip("Segundos a esperar en pantalla de GameOver antes de cambiar de escena")]
    [SerializeField] private float gameOverDelay = 3.0f;

    [Header("Configuración de Respaldo")]
    [Tooltip("Vidas por defecto si el GameManager entrega un valor menor o igual a 0")]
    [SerializeField] private int fallbackLives = 3;

    public int CurrentScore { get; private set; }
    public int CurrentLives { get; private set; }
    public GameState CurrentState { get; private set; }

    // Eventos del Framework para la Capa de Juego y la UI
    public event Action<GameState> OnGameStateChanged;
    public event Action<int> OnScoreChanged;
    public event Action<int> OnLivesChanged;

    private void Awake()
    {
        string currentObjName = gameObject.name;
        string currentSceneName = gameObject.scene.name;
        int currentID = GetInstanceID();

        string existingInstanceInfo = (Instance != null)
        ? $"Objeto '{Instance.gameObject.name}' (Scene: '{Instance.gameObject.scene.name}', ID: {Instance.GetInstanceID()})"
        : "NULL";

        #if UNITY_EDITOR
        Debug.Log($"<color=yellow>[LevelManager Awake]</color> Ejecutando en: <b>'{currentObjName}'</b> (Scene: <b>'{currentSceneName}'</b>, ID: <b>{currentID}</b>) | Instance actual apunta a: <b>{existingInstanceInfo}</b>");
        #endif

        if (Instance != null && Instance != this)
        {
            #if UNITY_EDITOR
            Debug.LogWarning($"<color=red>[LevelManager DESTRUIDO]</color> Se destruye el componente en: <b>'{currentObjName}'</b> (ID: <b>{currentID}</b>) porque 'Instance' ya pertenecía a: <b>{existingInstanceInfo}</b>");
            #endif
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        #if UNITY_EDITOR
        Debug.Log($"<color=green>[LevelManager Start]</color> Inicializado con éxito en: <b>'{gameObject.name}'</b> (Scene: <b>'{gameObject.scene.name}'</b>, ID: <b>{GetInstanceID()}</b>)");
        #endif

        if (GameManager.Instance != null && GameManager.Instance.totalLives > 0)
        {
            CurrentScore = GameManager.Instance.globalScore;
            CurrentLives = GameManager.Instance.totalLives;
        }
        else
        {
            CurrentLives = fallbackLives;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetLives(CurrentLives);
            }
        }

        OnScoreChanged?.Invoke(CurrentScore);
        OnLivesChanged?.Invoke(CurrentLives);

        SetGameState(GameState.Playing);
    }

    private void OnDestroy()
{
#if UNITY_EDITOR
    Debug.Log($"<color=orange>[LevelManager OnDestroy]</color> OnDestroy ejecutado en: <b>'{gameObject.name}'</b> (ID: <b>{GetInstanceID()}</b>)\n<b>Pila de llamadas:</b>\n{System.Environment.StackTrace}");
#endif

    if (Instance == this)
    {
        Instance = null;
    }
}


    /// <summary>
    /// Cambia el estado del juego y notifica a los suscriptores (Capa 2 / UI).
    /// </summary>
    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
        Time.timeScale = (CurrentState == GameState.Paused) ? 0f : 1f;

#if UNITY_EDITOR
        Debug.Log($"<color=cyan>[LevelManager Debug]</color> Cambio de Estado: ---> <b>{CurrentState}</b> | Time.timeScale: {Time.timeScale}");
#else
        Debug.Log($"[LevelManager] Cambio de Estado: ---> {CurrentState} | Time.timeScale: {Time.timeScale}");
#endif

        // Notificar a todos los sistemas de Capa 2 (SpawnController, UI, etc.) de forma desacoplada
        OnGameStateChanged?.Invoke(CurrentState);

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
        if (CurrentScore < 0) CurrentScore = 0;

        OnScoreChanged?.Invoke(CurrentScore);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetScore(CurrentScore);
        }
    }

    public void ApplyDamage(int damage)
    {
        CurrentLives -= damage;
        if (CurrentLives < 0) CurrentLives = 0;

        OnLivesChanged?.Invoke(CurrentLives);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetLives(CurrentLives);
        }

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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetToDefaultValues();
            GameManager.Instance.currentLevelIndex = 1;
        }

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
        int nextLevel = (GameManager.Instance != null) ? GameManager.Instance.currentLevelIndex + 1 : 1;
        int totalBuildScenes = SceneManager.sceneCountInBuildSettings;

        if (nextLevel < totalBuildScenes)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentLevelIndex = nextLevel;
            }
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            ReturnToMenuScene();
        }
    }
}
