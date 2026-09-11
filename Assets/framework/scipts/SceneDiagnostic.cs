using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDiagnostic : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"<color=cyan>[SceneDiagnostic]</color> Escena CARGADA: <b>'{scene.name}'</b> | Modo: {mode}");
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"<color=red>[SceneDiagnostic]</color> Escena DESCARGADA: <b>'{scene.name}'</b>\n<b>Pila de llamadas:</b>\n{System.Environment.StackTrace}");
    }
}