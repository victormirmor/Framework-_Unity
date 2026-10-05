using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
    private static Player inputActions;

    private static Player Actions{
        get{
            if (inputActions == null){
                inputActions = new Player();
                inputActions.movement.Enable();
                inputActions.UI.Enable();
            }
            return inputActions;
        }
    }

    /// Retorna el vector de movimiento 2D (Action: move)
    public static Vector2 GetMoveVector() => Actions.movement.move.ReadValue<Vector2>();

    /// Retorna el vector de cámara 2D (Action: Camera)
    public static Vector2 GetCameraVector() => Actions.movement.Camera.ReadValue<Vector2>();

    /// Compatibilidad con ejes clásicos leyendo movimiento (Horizontal/Vertical) y cámara (MouseX/MouseY).
    public static float GetAxis(string axisName)
    {
        Vector2 moveVector = GetMoveVector();
        Vector2 cameraVector = GetCameraVector();

        switch (axisName){
            // Movimiento principal
            case "Horizontal": return moveVector.x;
            case "Vertical":   return moveVector.y;

            // Ejes de Cámara / Mouse
            case "MouseX":     return cameraVector.x;
            case "MouseY":     return cameraVector.y;
            case "CameraX":    return cameraVector.x;
            case "CameraY":    return cameraVector.y;

            default:
                Debug.LogWarning($"[InputManager] El eje '{axisName}' no está mapeado.");
                return 0f;
        }
    }

    /// Detecta si el botón se presionó en el frame actual (WasPressedThisFrame).
    public static bool GetButtonDown(string buttonName){
        switch (buttonName){
            // Acciones de disparo / ataque
            case "Fire1":       return Actions.movement.Shoot1.WasPressedThisFrame();
            case "Fire2":       return Actions.movement.Shoot2.WasPressedThisFrame();
            case "Shoot1":      return Actions.movement.Shoot1.WasPressedThisFrame();
            case "Shoot2":      return Actions.movement.Shoot2.WasPressedThisFrame();

            // Acciones primarias
            case "Action1":     return Actions.movement.Action1.WasPressedThisFrame();
            case "Action2":     return Actions.movement.Action2.WasPressedThisFrame();
            case "Action3":     return Actions.movement.Action3.WasPressedThisFrame();
            case "Action4":     return Actions.movement.Action4.WasPressedThisFrame();

            // Rotaciones
            case "RotateLeft":  return Actions.movement.rotate_left.WasPressedThisFrame();
            case "RotateRight": return Actions.movement.rotate_right.WasPressedThisFrame();
            case "rotate_left": return Actions.movement.rotate_left.WasPressedThisFrame();
            case "rotate_right":return Actions.movement.rotate_right.WasPressedThisFrame();

            // Gatillos / Adicionales
            case "AditionalL":  return Actions.movement.AditionalL.WasPressedThisFrame();
            case "AditionalR":  return Actions.movement.AditionalR.WasPressedThisFrame();

            // Menús / Vistas
            case "Inventory":   return Actions.movement.Inventary.WasPressedThisFrame();
            case "Map":         return Actions.movement.map.WasPressedThisFrame();
            case "Cancel":      return Actions.UI.Cancel.WasPressedThisFrame();
            case "Submit":      return Actions.UI.Submit.WasPressedThisFrame();

            default:
                Debug.LogWarning($"[InputManager] El botón Down '{buttonName}' no está mapeado.");
                return false;
        }
    }

    /// Detecta si el botón se mantiene presionado (IsPressed).
    public static bool GetButton(string buttonName){
        switch (buttonName)
        {
            case "Fire1":       return Actions.movement.Shoot1.IsPressed();
            case "Fire2":       return Actions.movement.Shoot2.IsPressed();
            case "Shoot1":      return Actions.movement.Shoot1.IsPressed();
            case "Shoot2":      return Actions.movement.Shoot2.IsPressed();
            case "Action1":     return Actions.movement.Action1.IsPressed();
            case "Action2":     return Actions.movement.Action2.IsPressed();
            case "Action3":     return Actions.movement.Action3.IsPressed();
            case "Action4":     return Actions.movement.Action4.IsPressed();
            case "RotateLeft":  return Actions.movement.rotate_left.IsPressed();
            case "RotateRight": return Actions.movement.rotate_right.IsPressed();
            case "AditionalL":  return Actions.movement.AditionalL.IsPressed();
            case "AditionalR":  return Actions.movement.AditionalR.IsPressed();
            default:
                Debug.LogWarning($"[InputManager] El botón Held '{buttonName}' no está mapeado.");
                return false;
        }
    }

    /// Detecta si el botón se soltó en el frame actual (WasReleasedThisFrame).
    public static bool GetButtonUp(string buttonName){
        switch (buttonName)
        {
            case "Fire1":       return Actions.movement.Shoot1.WasReleasedThisFrame();
            case "Fire2":       return Actions.movement.Shoot2.WasReleasedThisFrame();
            case "Shoot1":      return Actions.movement.Shoot1.WasReleasedThisFrame();
            case "Shoot2":      return Actions.movement.Shoot2.WasReleasedThisFrame();
            case "Action1":     return Actions.movement.Action1.WasReleasedThisFrame();
            case "Action2":     return Actions.movement.Action2.WasReleasedThisFrame();
            case "Action3":     return Actions.movement.Action3.WasReleasedThisFrame();
            case "Action4":     return Actions.movement.Action4.WasReleasedThisFrame();
            default:
                Debug.LogWarning($"[InputManager] El botón Up '{buttonName}' no está mapeado.");
                return false;
        }
    }
}