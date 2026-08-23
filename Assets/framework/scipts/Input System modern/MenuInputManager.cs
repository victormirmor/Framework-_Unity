using UnityEngine;

public class MenuInputManager : MonoBehaviour
{
    private bool pausePressedLastFrame = false;

    private void Update()
    {
        if (LevelManager.Instance == null || InputDataMap.Instance == null) return;

        bool currentPauseState = InputDataMap.Instance.actionPause;

        // Solo actuamos en el frame exacto en que se PRESIONA el botón (Edge Trigger)
        if (currentPauseState && !pausePressedLastFrame)
        {
            // Solo alternar si el juego ya pasó la pantalla de idioma y está jugando o pausado
            if (LevelManager.Instance.CurrentState == GameState.Playing || 
                LevelManager.Instance.CurrentState == GameState.Paused)
            {
                LevelManager.Instance.TogglePause();
            }
        }

        pausePressedLastFrame = currentPauseState;
    }
}