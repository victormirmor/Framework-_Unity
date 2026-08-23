using UnityEngine;

[CreateAssetMenu(fileName = "Localization_ES", menuName = "Framework/Localization/Language Data")]
public class LocalizationDataSO : ScriptableObject
{
    [Header("Identificación del Idioma")]
    public string languageName = "Español"; //[cite: 9]
    public string languageCode = "es"; //[cite: 9]

    [Header("Textos de Pantalla / UI")]
    public string gameOverText = "¡HAS PERDIDO!"; //[cite: 9]
    public string gameWinText = "¡VICTORIA!"; //[cite: 9]
    public string pausedText = "PAUSA"; //[cite: 9]

    [Header("Textos de Gameplay")]
    public string livesRemainingText = "VIDAS RESTANTES: "; //[cite: 9]

    [Header("Textos de Menú Principal")]
    public string playButtonText = "JUGAR";
    public string optionsButtonText = "OPCIONES";
    public string quitButtonText = "SALIR";
}