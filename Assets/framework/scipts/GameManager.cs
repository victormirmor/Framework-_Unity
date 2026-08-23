using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string HIGH_SCORE_PREFIX = "HighScore_";

    public static GameManager Instance { get; private set; }

    [Header("Escena Inicial / Menú")]
    [SerializeField] private string resetSceneName = "main_menu";

    [Header("Configuración por Defecto")]
    [SerializeField] private GameSettingsSO defaultSettings;

    [Header("Modo de Juego Activo")]
    public GameMode currentMode = GameMode.Classic;

    [Header("Perfil Persistente del Jugador")]
    public float currentHealth;
    public int totalLives;
    public int globalScore;
    public int currentLevelIndex;

    [Header("Idioma Activo")]
    public LocalizationDataSO currentLanguage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeGame();
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals(resetSceneName) || scene.buildIndex == 0)
        {
            ResetToDefaultValues();
        }
    }

    public void InitializeGame()
    {
        ResetToDefaultValues();
        
#if UNITY_EDITOR
        DebugAllModeHighScores();
#endif
    }

    public void ResetToDefaultValues()
    {
        Time.timeScale = 1f;

        if (defaultSettings != null)
        {
            currentHealth = defaultSettings.defaultHealth;
            totalLives = defaultSettings.defaultLives;
            globalScore = defaultSettings.defaultScore;
            currentLevelIndex = defaultSettings.defaultLevelIndex;

            if (currentLanguage == null)
            {
                currentLanguage = defaultSettings.defaultLanguage;
            }
        }

        Debug.Log($"[GameManager] Datos reseteados para una nueva sesión | Time.timeScale: {Time.timeScale}");
    }

    public void SetGameMode(GameMode mode)
    {
        currentMode = mode;

#if UNITY_EDITOR
        string prefKey = HIGH_SCORE_PREFIX + currentMode.ToString();
        bool exists = PlayerPrefs.HasKey(prefKey);
        Debug.Log($"<color=cyan>[Editor Debug]</color> Modo asignado: <b>{currentMode}</b> | Clave PlayerPrefs: '{prefKey}' | ¿Existe en disco?: {exists} | Récord actual: {GetHighScore(currentMode)}");
#endif
    }

    public int GetHighScore(GameMode mode)
    {
        string prefKey = HIGH_SCORE_PREFIX + mode.ToString();
        return PlayerPrefs.GetInt(prefKey, 0);
    }

    public void SetScore(int score)
    {
        globalScore = score;

        int activeHighScore = GetHighScore(currentMode);

        if (globalScore > activeHighScore)
        {
            string prefKey = HIGH_SCORE_PREFIX + currentMode.ToString();
            PlayerPrefs.SetInt(prefKey, globalScore);
            PlayerPrefs.Save();

#if UNITY_EDITOR
            Debug.Log($"<color=green>[Editor Debug]</color> ¡Nuevo récord guardado en PlayerPrefs para <b>{currentMode}</b>! Clave: '{prefKey}' | Valor: {globalScore}");
#endif
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Recorre todos los modos del enum GameMode e imprime en la consola de Unity si sus PlayerPrefs existen.
    /// </summary>
    private void DebugAllModeHighScores()
    {
        GameMode[] allModes = (GameMode[])System.Enum.GetValues(typeof(GameMode));
        
        Debug.Log("<color=yellow>[Editor Debug]</color> --- ESTADO INICIAL DE PLAYERPREFS POR MODO ---");
        
        foreach (GameMode mode in allModes)
        {
            string prefKey = HIGH_SCORE_PREFIX + mode.ToString();
            bool exists = PlayerPrefs.HasKey(prefKey);
            int value = PlayerPrefs.GetInt(prefKey, 0);
            
            Debug.Log($"Modo: <b>{mode}</b> | Clave: '{prefKey}' | Guardado: {(exists ? $"SÍ ({value} pts)" : "NO (Aún no creado)")}");
        }
    }
#endif

    public void SetLanguage(LocalizationDataSO newLanguage)
    {
        if (newLanguage == null) return;
        currentLanguage = newLanguage;
    }

    public void SetHealth(float health) => currentHealth = health;
    public void SetLives(int lives) => totalLives = lives;
}