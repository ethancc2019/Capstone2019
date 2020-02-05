using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {


	//variable for flying speed
	public float speed;
	//reference for Rigidbody2D
	Rigidbody2D rb;

	private float maxRotationSpeed;
	//private Vector3 rotSpeed;
	public float degreesPerSec = 360f;


	//will be executed once at script start
	void Start()
	{
		//reference to Rigidbody2D
		rb = GetComponent<Rigidbody2D>();
		//declare direction vector for moving (this will be along the Y-axis)
		Vector3 move = new Vector3(0, -1, 0);
		//change velocity (moving speed and direction)
		rb.velocity = move * speed;

		maxRotationSpeed = 50;
		//rotSpeed = Random.insideUnitSphere * maxRotationSpeed;
	}



	// Update is called once per frame
	void Update () {

		float rotAmount = degreesPerSec * Time.deltaTime;
		float curRot = transform.localRotation.eulerAngles.z;
		transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
	}
}
