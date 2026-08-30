using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        Victory
    }

    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [Header("Anotador / Apuntador de Datos")]
        [SerializeField] protected GameSessionDataSO sessionData;

        [Header("Configuración de Transición")]
        [SerializeField] protected string menuSceneName = "main_menu";
        [SerializeField] protected float gameOverDelay = 3.0f;
        [SerializeField] protected float maxHealth = 100f;

        public GameState CurrentState { get; protected set; }

        public event Action<GameState> OnGameStateChanged;

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        protected virtual void Start()
        {
            SetGameState(GameState.Playing);

            if (sessionData != null)
            {
#if UNITY_EDITOR
                Debug.Log($"<color=cyan>[GameController DEBUG]</color> Sincronizando sesión desde: <b>{sessionData.name}</b>");
#endif
                sessionData.NotifyAllCurrentValues();
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"<color=red>[GameController DEBUG]</color> ERROR: 'sessionData' NO asignado en el Inspector.", gameObject);
#endif
            }
        }

        public virtual void AddScore(int points, string modeKey = "Default")
        {
            if (sessionData == null) return;

#if UNITY_EDITOR
            Debug.Log($"<color=green>[GameController DEBUG]</color> Incrementando puntos (+{points})");
#endif
            int newScore = sessionData.GlobalScore + points;
            sessionData.CheckAndSaveHighScore(modeKey, newScore);
        }

        public virtual void LoseLife(int amount = 1)
        {
            if (sessionData == null) return;

#if UNITY_EDITOR
            Debug.Log($"<color=orange>[GameController DEBUG]</color> Restando vida (-{amount})");
#endif
            sessionData.TotalLives -= amount;

            if (sessionData.TotalLives <= 0)
            {
                SetGameState(GameState.GameOver);
            }
        }

        public virtual void ApplyDamage(float damage)
        {
            if (sessionData == null || sessionData.complexityLevel < SessionComplexityLevel.Level1_StandardAction) return;

#if UNITY_EDITOR
            Debug.Log($"<color=red>[GameController DEBUG]</color> Aplicando daño (-{damage})");
#endif
            sessionData.CurrentHealth = Mathf.Max(0f, sessionData.CurrentHealth - damage);

            if (sessionData.CurrentHealth <= 0f)
            {
                LoseLife(1);

                if (sessionData.TotalLives > 0)
                {
                    sessionData.CurrentHealth = maxHealth;
                }
            }
        }

        public virtual void SetGameState(GameState newState)
        {
            CurrentState = newState;
            Time.timeScale = (CurrentState == GameState.Paused) ? 0f : 1f;

#if UNITY_EDITOR
            Debug.Log($"<color=purple>[GameController DEBUG]</color> Transición de Estado de Juego -> <b>{CurrentState}</b>");
#endif
            OnGameStateChanged?.Invoke(CurrentState);

            if (CurrentState == GameState.GameOver)
            {
                Invoke(nameof(ReturnToMenuScene), gameOverDelay);
            }
        }

        protected virtual void ReturnToMenuScene()
        {
            if (sessionData != null)
            {
                sessionData.currentLevelIndex = 1;
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
    }
}