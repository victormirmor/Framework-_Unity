using UnityEngine;

namespace Framework
{
    public class SessionResetInitializer : MonoBehaviour
    {
        [Header("Anotador Pasivo / Apuntador de Datos")]
        [SerializeField] private GameSessionDataSO sessionData;

        [Header("Configuración de Reset")]
        [SerializeField] private bool resetOnStart = true;

        private void Start()
        {
            if (resetOnStart && sessionData != null)
            {
#if UNITY_EDITOR
                Debug.Log($"<color=yellow>[SessionResetInitializer DEBUG]</color> Menú detectado. Restableciendo 'GameSessionData' a valores por defecto.");
#endif
                sessionData.ResetSession();
            }
        }
    }
}