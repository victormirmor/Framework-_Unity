using UnityEngine;
using MiJuego.InputAdaptador;

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

		private void Update(){


			if (Time.time > nextFire && CoutBullets > 0 && CrossPlatformInputManager.GetButtonDown("Fire1")){
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
