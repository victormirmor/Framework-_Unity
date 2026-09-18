using UnityEngine;
using System.Collections;

public class Done_Mover : MonoBehaviour
{
	public float speed;
	public Vector3 VTranslate;
	public Rigidbody Rb;

	void Start ()
	{
		Rb=GetComponent<Rigidbody>();
		translate();
	}

	void translate(){
		Rb.velocity = VTranslate * speed;
	}
}
