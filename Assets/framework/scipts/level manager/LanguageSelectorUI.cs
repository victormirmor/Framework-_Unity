using UnityEngine;

public class LanguageSelectorUI : MonoBehaviour
{
    public const string LANGUAGE_PREF_KEY = "SelectedLanguageIndex";

    [Header("Referencias de UI")]
    [SerializeField] private GameObject languagePanel;

    [Header("Lista de Idiomas Disponibles")]
    [Tooltip("0 = Español, 1 = Inglés, 2 = Portugués, etc.")]
    [SerializeField] private LocalizationDataSO[] availableLanguages;

    private MainMenuUI mainMenuUI;

    private void Awake()
    {
        mainMenuUI = Object.FindFirstObjectByType<MainMenuUI>();

        // Si ya hay preferencia guardada, la aplicamos al GameManager y al Menú
        if (PlayerPrefs.HasKey(LANGUAGE_PREF_KEY))
        {
            int savedIndex = PlayerPrefs.GetInt(LANGUAGE_PREF_KEY, 0);

            if (availableLanguages != null && savedIndex >= 0 && savedIndex < availableLanguages.Length)
            {
                LocalizationDataSO savedLang = availableLanguages[savedIndex];

                if (mainMenuUI != null)
                    mainMenuUI.UpdateMainMenuLanguage(savedLang);

                if (GameManager.Instance != null)
                    GameManager.Instance.SetLanguage(savedLang);
            }

//#if !UNITY_EDITOR
            ClosePanel();
//#endif
        }
    }

    public void SelectLanguageByIndex(int index)
    {
        if (availableLanguages == null || availableLanguages.Length == 0) return;

        if (index >= 0 && index < availableLanguages.Length)
        {
            LocalizationDataSO selectedLang = availableLanguages[index];

            // 1. Guardar preferencia local
            PlayerPrefs.SetInt(LANGUAGE_PREF_KEY, index);
            PlayerPrefs.Save();

            // 2. Notificar al GameManager (Persistente)
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetLanguage(selectedLang);
            }

            // 3. Actualizar la UI del Menú Principal
            if (mainMenuUI != null)
            {
                mainMenuUI.UpdateMainMenuLanguage(selectedLang);
            }

            ClosePanel();
        }
    }

    private void ClosePanel()
    {
        if (languagePanel != null)
            languagePanel.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}
