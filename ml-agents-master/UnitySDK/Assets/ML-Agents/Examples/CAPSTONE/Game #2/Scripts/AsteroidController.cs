using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{


    public GameObject asteroid;
    private GameObject asteroid_temp;

    private float start_time = 10f; //Spawn asteroids in 10 seconds
    private float min = 0.0f;
    private float max = 0.8f;

    // Use this for initialization
    void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    start_time -= Time.deltaTime;
	    if (start_time <= 0)
	    {
	        asteroid_temp = asteroid;
            Vector3 randomPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(min, max), 1,10));
	        Instantiate(asteroid,
	            new Vector3(Random.Range(-9.0f, 9.0f),
	                Random.Range(-6.0f, 6.0f), 0),
	            Quaternion.Euler(0, 0, Random.Range(-0.0f, 359.0f)));
            asteroid_temp.GetComponent<Rigidbody2D>().AddForce(transform.up * Random.Range(-50.0f, 150.0f));
            start_time = 10f;
	    }
        Debug.Log(start_time.ToString());
	}
}
