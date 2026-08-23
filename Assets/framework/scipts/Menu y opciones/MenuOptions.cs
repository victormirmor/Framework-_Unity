using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour 
{
    [Header("Dropdowns Legacy de Configuración")]
    public Dropdown DropdownResolution;
    public Dropdown DropdownQuality;
    public Dropdown DropdownShadows;
    public Dropdown DropdownVsync;
    public Dropdown DropdownantiAliasing;
    public Dropdown DropdownScreen;

    private void Start()
    {
        if (DropdownResolution != null) DropdownResolution.value = PlayerPrefs.GetInt("Resolution", 2);
        if (DropdownantiAliasing != null) DropdownantiAliasing.value = PlayerPrefs.GetInt("antiAliasing", 0);        
        if (DropdownQuality != null) DropdownQuality.value = PlayerPrefs.GetInt("Quality", 2);
        if (DropdownShadows != null) DropdownShadows.value = PlayerPrefs.GetInt("Shadows", 2);
        if (DropdownScreen != null) DropdownScreen.value = PlayerPrefs.GetInt("Screen", 1);
        if (DropdownVsync != null) DropdownVsync.value = PlayerPrefs.GetInt("Vsync", 0);
    }

    private void OnDisable()
    {
        if (DropdownResolution != null) PlayerPrefs.SetInt("Resolution", DropdownResolution.value);
        if (DropdownantiAliasing != null) PlayerPrefs.SetInt("antiAliasing", DropdownantiAliasing.value);
        if (DropdownQuality != null) PlayerPrefs.SetInt("Quality", DropdownQuality.value);
        if (DropdownShadows != null) PlayerPrefs.SetInt("Shadows", DropdownShadows.value);
        if (DropdownScreen != null) PlayerPrefs.SetInt("Screen", DropdownScreen.value);
        if (DropdownVsync != null) PlayerPrefs.SetInt("Vsync", DropdownVsync.value);

        PlayerPrefs.Save();
    }

    public void SetResolution(int level)
    {
        switch (level)
        {
            case 0: Screen.SetResolution(848, 480, Screen.fullScreen); break;
            case 1: Screen.SetResolution(1024, 600, Screen.fullScreen); break;
            case 2: Screen.SetResolution(1280, 720, Screen.fullScreen); break;
            case 3: Screen.SetResolution(1920, 1080, Screen.fullScreen); break;
        }
    }   

    public void Get_Quality(int level)
    {
        QualitySettings.SetQualityLevel(level, true);
    }

    public void ShadowsLevel(int level)
    {
        QualitySettings.shadows = (ShadowQuality)level;
    }

    public void Vsync(int level)
    {
        QualitySettings.vSyncCount = level;
    }

    public void Set_antiAliasing(int level)
    {
        int[] samples = { 0, 2, 4, 8 };
        if (level >= 0 && level < samples.Length)
        {
            QualitySettings.antiAliasing = samples[level];
        }
    }

    public void FullScreen(int level)
    {
        Screen.fullScreen = (level == 1);
    }
}