using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Main_Menu : MonoBehaviour
{
    [Header("Referencias de Paneles / Canvas")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    private void Start()
    {
        // El menú principal siempre requiere cursor visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        ShowMainMenu();
    }

    public void LoadScene(string sceneName)
    { 
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleOptions(bool showOptions) 
    {
        if (optionsPanel) optionsPanel.SetActive(showOptions);
        if (mainMenuPanel) mainMenuPanel.SetActive(!showOptions);
    }

    public void ShowMainMenu()
    {
        ToggleOptions(false);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR 
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}