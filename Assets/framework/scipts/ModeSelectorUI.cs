using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode{
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
    [SerializeField] private string sceneToLoad = "Level_01";

    /// <summary>
    /// Método público sin parámetros asignable directamente al OnClick() del Button.
    /// </summary>
    public void OnSelectModeButtonPressed()
    {
        ExecuteModeSelection();
    }

    /// <summary>
    /// Método por si querés invocar la selección vía código desde otro script.
    /// </summary>
    public void SelectModeCustom(GameMode mode, string sceneName)
    {
        selectedMode = mode;
        sceneToLoad = sceneName;
        ExecuteModeSelection();
    }

    private void ExecuteModeSelection()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetGameMode(selectedMode);
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError($"[ModeSelectorUI] No se especificó una escena válida en el objeto {gameObject.name}.");
        }
    }
}