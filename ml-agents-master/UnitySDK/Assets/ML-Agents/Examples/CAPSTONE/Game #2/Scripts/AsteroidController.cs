using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidController : MonoBehaviour
{


    public GameObject asteroid;
    private GameObject asteroid_temp;

    private float start_time = 10f; //Spawn asteroids in 10 seconds
    private float min = 0.0f;
    private float max = 0.8f;


    private int waveNum;
    private int nunmOfAsteroids;

    private Text waveText;

	public GameObject[] asteroidSpawns;
    // Use this for initialization
    void Start ()
    {
        waveNum = 1;
        waveText = GameObject.FindGameObjectWithTag("wave_text").GetComponent<Text>();
        waveText.text = waveNum.ToString();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    start_time -= Time.deltaTime;
	    if (start_time <= 0)
	    {
			//SpawnAsteroids();
	    }
	}

    public void SpawnAsteroids()
    {

        Debug.Log("Asteroid incoming!");

        var ran = Random.Range(0, asteroidSpawns.Length);
        var spawnLoc = asteroidSpawns[ran].transform.position;
        asteroid_temp = asteroid;
        var randomPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(min, max), 1, 10));
        Instantiate(asteroid, spawnLoc, Quaternion.Euler(0, 0, Random.Range(-0.0f, 359.0f)));
        asteroid_temp.GetComponent<Rigidbody2D>().AddForce(transform.up * Random.Range(-50.0f, 150.0f));

        //Reset the spawn timer
        start_time = 10f;

        waveNum++;// Increase wave
        //update text
        waveText.text = waveNum.ToString();
    }
}
