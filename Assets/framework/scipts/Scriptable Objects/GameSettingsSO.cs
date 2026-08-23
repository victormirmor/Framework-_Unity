using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Framework/Settings/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Localización")]
    [Tooltip("Archivo de idioma activo por defecto.")]
    public LocalizationDataSO defaultLanguage;

    [Header("Valores Iniciales del Jugador")]
    public float defaultHealth = 100f;
    public int defaultLives = 8;
    public int defaultScore = 0;
    public int defaultLevelIndex = 1;
    
    [Header("Localización Global")]
public LocalizationDataSO[] availableLanguages;
}