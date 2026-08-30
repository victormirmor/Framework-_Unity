using System;
using UnityEngine;

namespace Framework
{
    public enum SessionComplexityLevel
    {
        Level0_BasicArcade,      // Vidas y Puntuación
        Level1_StandardAction,   // Vidas, Puntuación, High Score y Salud
        Level2_FullSurvivalRPG   // Todos los sistemas
    }

    [CreateAssetMenu(fileName = "GameSessionData", menuName = "Framework/Session Data")]
    public class GameSessionDataSO : ScriptableObject
    {
        public const string LANGUAGE_PREF_KEY = "SelectedLanguageIndex";
        private const string HIGH_SCORE_PREFIX = "HighScore_";

        [Header("Configuración de Modularidad")]
        public SessionComplexityLevel complexityLevel = SessionComplexityLevel.Level0_BasicArcade;

        [Header("Configuración de Idioma")]
        public LocalizationDataSO currentLanguage;
        [SerializeField] private LocalizationDataSO defaultLanguage;

        // --- CAMPOS PRIVADOS ---
        [SerializeField] private int totalLives = 3;
        [SerializeField] private int globalScore = 0;
        [SerializeField] private float currentHealth = 100f;
        [SerializeField] private int highScore = 0;

        [Header("Valores por Defecto (Reset)")]
        [SerializeField] private int defaultLives = 3;
        [SerializeField] private int defaultScore = 0;
        [SerializeField] private float defaultHealth = 100f;
        [SerializeField] private int defaultLevelIndex = 1;

        public int currentLevelIndex;

        // --- EVENTOS PÚBLICOS ---
        public event Action<int> OnLivesChanged;
        public event Action<int> OnScoreChanged;
        public event Action<float> OnHealthChanged;
        public event Action<LocalizationDataSO> OnLanguageChanged;

        // --- PROPIEDADES CON NOTIFICACIÓN AUTOMÁTICA ---
        public int TotalLives
        {
            get => totalLives;
            set
            {
                totalLives = value;
#if UNITY_EDITOR
                Debug.Log($"<color=orange>[GameSessionDataSO DEBUG]</color> TotalLives actualizado -> <b>{totalLives}</b>. Emitiendo a {OnLivesChanged?.GetInvocationList().Length ?? 0} suscriptor(es).");
#endif
                OnLivesChanged?.Invoke(totalLives);
            }
        }

        public int GlobalScore
        {
            get => globalScore;
            set
            {
                globalScore = value;
#if UNITY_EDITOR
                Debug.Log($"<color=green>[GameSessionDataSO DEBUG]</color> GlobalScore actualizado -> <b>{globalScore}</b>. Emitiendo a {OnScoreChanged?.GetInvocationList().Length ?? 0} suscriptor(es).");
#endif
                OnScoreChanged?.Invoke(globalScore);
            }
        }

        public float CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
#if UNITY_EDITOR
                Debug.Log($"<color=red>[GameSessionDataSO DEBUG]</color> CurrentHealth actualizado -> <b>{currentHealth}</b>. Emitiendo a {OnHealthChanged?.GetInvocationList().Length ?? 0} suscriptor(es).");
#endif
                OnHealthChanged?.Invoke(currentHealth);
            }
        }

        public virtual void ResetSession()
        {
#if UNITY_EDITOR
            Debug.Log($"<color=yellow>[GameSessionDataSO DEBUG]</color> Restableciendo valores de sesión por defecto...");
#endif
            currentLevelIndex = defaultLevelIndex;
            TotalLives = defaultLives;
            GlobalScore = defaultScore;

            if (complexityLevel >= SessionComplexityLevel.Level1_StandardAction)
            {
                CurrentHealth = defaultHealth;
            }

            if (currentLanguage == null && defaultLanguage != null)
            {
                SetLanguage(defaultLanguage);
            }
        }

        public virtual void CheckAndSaveHighScore(string gameModeKey, int newScore)
        {
            GlobalScore = newScore;
            string prefKey = HIGH_SCORE_PREFIX + gameModeKey;
            int activeHighScore = PlayerPrefs.GetInt(prefKey, 0);

            if (GlobalScore > activeHighScore)
            {
                highScore = GlobalScore;
                PlayerPrefs.SetInt(prefKey, GlobalScore);
                PlayerPrefs.Save();
#if UNITY_EDITOR
                Debug.Log($"<color=gold>[GameSessionDataSO DEBUG]</color> ¡Nuevo Récord Guardado para '{gameModeKey}'! Puntaje: <b>{highScore}</b>");
#endif
            }
        }

        public void NotifyAllCurrentValues()
        {
#if UNITY_EDITOR
            Debug.Log($"<color=cyan>[GameSessionDataSO DEBUG]</color> Forzando notificación masiva de valores iniciales a todos los apuntadores.");
#endif
            OnLivesChanged?.Invoke(totalLives);
            OnScoreChanged?.Invoke(globalScore);

            if (complexityLevel >= SessionComplexityLevel.Level1_StandardAction)
            {
                OnHealthChanged?.Invoke(currentHealth);
            }

            if (currentLanguage != null)
            {
                OnLanguageChanged?.Invoke(currentLanguage);
            }
        }

        public void SetLanguage(LocalizationDataSO newLanguage)
        {
            if (newLanguage != null)
            {
                currentLanguage = newLanguage;
#if UNITY_EDITOR
                Debug.Log($"<color=magenta>[GameSessionDataSO DEBUG]</color> Idioma cambiado a: <b>{currentLanguage.name}</b>");
#endif
                OnLanguageChanged?.Invoke(currentLanguage);
            }
        }
    }
}