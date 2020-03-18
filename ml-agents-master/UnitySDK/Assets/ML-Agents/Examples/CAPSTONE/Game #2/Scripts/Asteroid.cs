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
    private Vector2 lastPlayerPosition;


    private GameObject playerGameObject;
    private Vector2 startLocationVector2;

	//will be executed once at script start
	void Start()
	{
        playerGameObject = transform.parent.gameObject;
	    lastPlayerPosition = playerGameObject.transform.position;
		//reference to Rigidbody2D
	    startLocationVector2 = transform.position;
		rb = GetComponent<Rigidbody2D>();
	    Vector2 tempDir = (Vector2)playerGameObject.transform.position - (Vector2)transform.position;

        rb.AddForce(tempDir * speed,ForceMode2D.Force);
        float randomSize = Random.Range(0.2f, 2f);
        transform.localScale = new Vector3(randomSize, randomSize, randomSize);
		//declare direction vector for moving (this will be along the Y-axis)
		

		maxRotationSpeed = 50;
		//rotSpeed = Random.insideUnitSphere * maxRotationSpeed;
	}

    void Update()
    {
        float rotAmount = degreesPerSec * Time.deltaTime;
        float curRot = transform.localRotation.eulerAngles.z;
        //transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
        
       // rb.AddForce(lastPlayerPosition.normalized * 1000, ForceMode2D.Force);


        //transform.position = Vector2.MoveTowards(startLocationVector2, lastPlayerPosition, 5f);
        //transform.position = Vector2.Lerp(transform.position, lastPlayerPosition, 5f);

    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("asteroid_barrier"))
        {
            Destroy(gameObject);
            //spawnPointScript.activePowerups--;
        }

        
    }


}
