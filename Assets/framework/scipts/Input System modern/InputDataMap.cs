using UnityEngine;

public class InputDataMap : MonoBehaviour
{
    public static InputDataMap Instance { get; private set; }

    [Header("Ejes Continuos")]
    public float horizontal;
    public float vertical;

    [Header("Acciones / Botones Estándar (Abstracción)")]
    public bool actionJump;     // Mapeado a "Jump" (Espacio / Botón A)
    public bool actionFire1;    // Mapeado a "Fire1" (Ctrl / Clic Izq / Botón X)
    public bool actionSubmit;   // Mapeado a "Submit" (Enter / Botón A)
    public bool actionCancel;   // Mapeado a "Cancel" (Escape / Botón B)
    public bool actionPause;    // Mapeado a "Cancel" (Escape / Start)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        // Lectura segura de ejes (siempre existen en Unity por defecto)
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // Lectura de acciones de botones
        actionJump   = Input.GetButton("Jump");
        actionFire1  = Input.GetButton("Fire1");
        actionSubmit = Input.GetButton("Submit");
        actionCancel = Input.GetButton("Cancel");

        // Pausa segura: lee "Cancel" (tecla Esc / Botón atrás del mando)
        actionPause  = Input.GetButtonDown("Cancel");
    }
}