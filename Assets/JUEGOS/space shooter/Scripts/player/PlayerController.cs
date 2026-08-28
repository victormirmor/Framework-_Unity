using UnityEngine;

namespace SpaceShooter{


public class PlayerController : MonoBehaviour{
	[Header("Configuración de Movimiento")]
	[SerializeField] [Range(1.0f,20f)]private float speed = 12f;
	[SerializeField] [Range(0.1f,2.0f)]private float rotate;

	private Rigidbody rb;
	private float horizontalInput,verticalInput;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		// Si no estamos en estado de juego activo, frenamos la velocidad
		if (LevelManager.Instance != null && LevelManager.Instance.CurrentState != GameState.Playing)
		{
			horizontalInput = 0f;
			verticalInput = 0f;
			return;
		}

		// Lectura de entrada
		horizontalInput=(InputDataMap.Instance != null) ?InputDataMap.Instance.horizontal:Input.GetAxisRaw("Horizontal");
		verticalInput=(InputDataMap.Instance != null) ? InputDataMap.Instance.vertical : Input.GetAxisRaw("Vertical");
	}

	private void FixedUpdate(){
		rb.velocity = new Vector3(horizontalInput, 0f,verticalInput)* speed;
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -rotate);
		}
	}
}
