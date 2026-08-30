using UnityEngine;

namespace Framework
{
    public class LanguageSelectorUI : MonoBehaviour
    {
        [Header("Anotador Pasivo de Datos")]
        [SerializeField] private GameSessionDataSO sessionData;

        [Header("Referencias de UI")]
        [SerializeField] private GameObject languagePanel;

        [Header("Lista de Idiomas Disponibles")]
        [SerializeField] private LocalizationDataSO[] availableLanguages;

        private MainMenuUI mainMenuUI;

        private void Awake()
        {
            mainMenuUI = Object.FindFirstObjectByType<MainMenuUI>();

            if (PlayerPrefs.HasKey(GameSessionDataSO.LANGUAGE_PREF_KEY))
            {
                int savedIndex = PlayerPrefs.GetInt(GameSessionDataSO.LANGUAGE_PREF_KEY, 0);

                if (availableLanguages != null && savedIndex >= 0 && savedIndex < availableLanguages.Length)
                {
                    ApplyLanguage(availableLanguages[savedIndex]);
                }

                ClosePanel();
            }
        }

        public void SelectLanguageByIndex(int index)
        {
            if (availableLanguages == null || availableLanguages.Length == 0) return;

            if (index >= 0 && index < availableLanguages.Length)
            {
                LocalizationDataSO selectedLang = availableLanguages[index];

                PlayerPrefs.SetInt(GameSessionDataSO.LANGUAGE_PREF_KEY, index);
                PlayerPrefs.Save();

                ApplyLanguage(selectedLang);
                ClosePanel();
            }
        }

        private void ApplyLanguage(LocalizationDataSO language)
        {
            if (sessionData != null)
            {
                sessionData.SetLanguage(language);
            }

            if (mainMenuUI != null)
            {
                mainMenuUI.UpdateMainMenuLanguage(language);
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
}
