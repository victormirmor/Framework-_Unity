using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Classic,
    Survival,
    Math,
    Arcade,
    TimeAttack
}

public class ModeSelectorUI : MonoBehaviour
{
    [Header("Configuración de la Tarjeta/Botón")]
    [Tooltip("Modo de juego que asignará este botón")]
    [SerializeField] private GameMode selectedMode;

    [Tooltip("Nombre de la escena que debe cargar este botón")]
    [SerializeField] private string sceneToLoad = "01_SpaceShooter";

    private static bool isLoadingScene = false;

    private void OnEnable()
    {
        isLoadingScene = false;
    }

    public void OnSelectModeButtonPressed()
    {
        ExecuteModeSelection();
    }

    public void SelectModeCustom(GameMode mode, string sceneName)
    {
        selectedMode = mode;
        sceneToLoad = sceneName;
        ExecuteModeSelection();
    }

    private void ExecuteModeSelection()
    {
        if (isLoadingScene) return;
        isLoadingScene = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetGameMode(selectedMode);
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
#if UNITY_EDITOR
            Debug.Log($"<color=green>[ModeSelectorUI]</color> Cargando escena: <b>'{sceneToLoad}'</b>");
#endif
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            isLoadingScene = false;
            Debug.LogError($"[ModeSelectorUI] No se especificó una escena válida en el objeto {gameObject.name}.");
        }
    }
}
