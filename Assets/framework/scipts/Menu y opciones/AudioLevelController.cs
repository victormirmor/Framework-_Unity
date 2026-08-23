using UnityEngine;

public class AudioLevelController : MonoBehaviour
{
    [Header("Fuentes de Audio del Nivel")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips de Música")]
    [SerializeField] private AudioClip levelBGM;
    [SerializeField] private AudioClip gameOverBGM;
    [SerializeField] private AudioClip victoryBGM;

    private void OnEnable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnGameStateChanged += HandleGameStateAudio;
        }
    }

    private void OnDisable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnGameStateChanged -= HandleGameStateAudio;
        }
    }

    private void HandleGameStateAudio(GameState state)
    {
        switch (state)
        {
            //case GameState.LanguageSelect:
                // Si querés una música suave de menú de inicio
                //break;

            case GameState.Playing:
                if (bgmSource != null && !bgmSource.isPlaying)
                {
                    bgmSource.clip = levelBGM;
                    bgmSource.UnPause();
                    if (!bgmSource.isPlaying) bgmSource.Play();
                }
                break;

            case GameState.Paused:
                if (bgmSource != null && bgmSource.isPlaying)
                {
                    bgmSource.Pause(); // Pausa la música durante el menú de pausa
                }
                break;

            case GameState.GameOver:
                PlayBGM(gameOverBGM, loop: false);
                break;

            case GameState.Victory:
                PlayBGM(victoryBGM, loop: false);
                break;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource == null || clip == null) return;
        
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }
}