using UnityEngine;
using System.Collections;

public class DestroyByTrigger : MonoBehaviour
{
	void OnTriggerEnter (Collider other){
		if (other.tag == "Enemy")
		{
			//int points=
			other.gameObject.GetComponent<EnemyData>();
			//Debug.Log("sumas "+points);

			Destroy(other.gameObject);
		}


	}
}
