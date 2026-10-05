using UnityEngine;

[CreateAssetMenu(fileName = "Localization_ES", menuName = "Framework/Localization/Language Data")]
public class LocalizationDataSO : ScriptableObject
{
    [Header("Identificación del Idioma")]
    public string languageName = "Español"; 
    public string languageCode = "es"; 
    [Header("Textos de Pantalla / UI")]
    public string gameOverText = "¡HAS PERDIDO!"; 
    public string gameWinText = "¡VICTORIA!"; 
    public string pausedText = "PAUSA"; 

    [Header("Textos de Gameplay")]
    public string livesRemainingText = "VIDAS RESTANTES: "; 
    [Header("Textos de Menú Principal")]
    public string playButtonText = "JUGAR";
    public string optionsButtonText = "OPCIONES";
    public string quitButtonText = "SALIR";
}