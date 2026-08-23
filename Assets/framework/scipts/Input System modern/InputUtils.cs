using UnityEngine;

public enum InputType { Modern, Legacy, Keyboard }

public static class InputUtils 
{
    private static int cachedJoystickCount = 0;
    private static string cachedJoystickName = string.Empty;
    private static InputType cachedInputType = InputType.Keyboard;

    /// <summary>
    /// Escanea los mandos conectados actualizando la caché local para evitar asignaciones en el Heap.
    /// </summary>
    public static void PollJoysticks()
    {
        string[] names = Input.GetJoystickNames();
        cachedJoystickCount = 0;
        cachedJoystickName = string.Empty;

        for (int i = 0; i < names.Length; i++)
        {
            if (!string.IsNullOrEmpty(names[i]))
            {
                cachedJoystickCount++;
                if (string.IsNullOrEmpty(cachedJoystickName))
                {
                    cachedJoystickName = names[i];
                }
            }
        }

        if (cachedJoystickCount == 0 || string.IsNullOrEmpty(cachedJoystickName))
        {
            cachedInputType = InputType.Keyboard;
        }
        else
        {
            string lowerName = cachedJoystickName.ToLower();
            if (lowerName.Contains("xbox") || 
                lowerName.Contains("xinput") ||
                lowerName.Contains("microsoft") ||
                lowerName.Contains("windows")) 
            {
                cachedInputType = InputType.Modern;
            }
            else
            {
                cachedInputType = InputType.Legacy;
            }
        }
    }

    public static int GetActiveJoystickCount() => cachedJoystickCount;
    public static string GetJoystickName() => cachedJoystickName;
    public static InputType GetDetectedInputType() => cachedInputType;

    /// <summary>
    /// Aplica una zona muerta radial vectorial eliminando el corte en cruz y reescalando la aceleración.
    /// </summary>
    public static Vector2 ApplyRadialDeadzone(Vector2 rawInput, float deadzone)
    {
        float magnitude = rawInput.magnitude;
        if (magnitude < deadzone)
            return Vector2.zero;

        return rawInput.normalized * ((magnitude - deadzone) / (1f - deadzone));
    }
}
