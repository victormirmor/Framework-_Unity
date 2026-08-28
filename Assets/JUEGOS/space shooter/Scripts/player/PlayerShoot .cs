using UnityEngine;

namespace SpaceShooter
{
	public class PlayerShoot : MonoBehaviour
	{
		public GameObject shot;
		public Transform shotSpawn;
		public float fireRate;
		public int CoutBullets = 20;
		private float nextFire;

		public AudioSource audioSource;

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		private void Update()
		{
			// Validamos que la instancia del mapeo de entrada exista
			if (InputDataMap.Instance == null){
				Debug.Log("no hay InputDataMap");
				}

			if (Time.time > nextFire && CoutBullets > 0 && InputDataMap.Instance.actionFire1){
				Debug.Log("disparo");
				Shoot();
			}
		}

		private void Shoot()
		{
			// Validaciones de seguridad en el inspector
			if (shot == null || shotSpawn == null)
			{
				Debug.LogError("Falta asignar 'shot' o 'shotSpawn' en el Inspector.");
				return;
			}

			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

			if (audioSource != null)
			{
				audioSource.Play();
			}

			CoutBullets--;
		}
	}
}
