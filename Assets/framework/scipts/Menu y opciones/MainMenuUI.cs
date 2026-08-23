using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [Header("Textos del Menú Principal (TextMeshPro)")]
    [SerializeField] private TMP_Text playButtonText;
    [SerializeField] private TMP_Text optionsButtonText;
    [SerializeField] private TMP_Text quitButtonText;

    // Método receptor invocado únicamente por LanguageSelectorUI
    public void UpdateMainMenuLanguage(LocalizationDataSO activeLanguage)
    {
        if (activeLanguage == null) return;

        if (playButtonText != null) 
            playButtonText.text = activeLanguage.playButtonText;

        if (optionsButtonText != null) 
            optionsButtonText.text = activeLanguage.optionsButtonText;

        if (quitButtonText != null) 
            quitButtonText.text = activeLanguage.quitButtonText;
    }
}