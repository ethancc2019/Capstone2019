using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidController : MonoBehaviour
{


    public GameObject[] asteroids; //Container for large and small asteroids. We will randomly select which one to spawn
    private GameObject asteroid_temp;

    private float spawn_time = 20f; //Spawn asteroids in 10 seconds
    //private float start_time = 5f; //TESTING
    private float min = 0.0f;
    private float max = 0.8f;
    private float asteroidSpeed = 5f;

    public GameObject player;

    private List<GameObject> activeAsteroids;



    private int waveNum;
    private int numOfAsteroids;

    public Text waveText;

	public GameObject[] asteroidSpawns;
    // Use this for initialization
    void Start ()
    {
        numOfAsteroids = 0;
        waveNum = 1;
        waveText.text = waveNum.ToString();

    }
	
	// Update is called once per frame
	void Update ()
	{
	    spawn_time -= Time.deltaTime;
	    if (spawn_time <= 0)
	    {
	        numOfAsteroids++;
            SpawnAsteroids();
	    }
        //asteroid_temp.GetComponent<Rigidbody2D>().AddForce(transform.forward * 500);
	}

    public void SpawnAsteroids()
    {

        //var ranAsteroid = Random.Range(0, asteroids.Length);
        //asteroid_temp = asteroids[ranAsteroid];
       // var randomPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(min, max), 1, 10));

        for (int i = 0; i < numOfAsteroids; i++)
        {
            var ran = Random.Range(0, asteroidSpawns.Length); //Get a random index
            var ranAsteroid = Random.Range(0, asteroids.Length); //Get a random Asteroid
            asteroid_temp = asteroids[ranAsteroid]; //Instantiate temp asteroid for Destroy() method
            var spawnLoc = asteroidSpawns[ran].transform.position; //Get random spawnLocation
            Instantiate(asteroid_temp, spawnLoc, Quaternion.identity, player.transform); //Make the asteroid
        }

        //Reset the spawn timer
        spawn_time = 20f;

        waveNum++;// Increase wave
        //update text
        waveText.text = waveNum.ToString();
    }
    
    public void ResetAsteroids()
    {
        numOfAsteroids = 0;
        waveNum = 1;
        waveText.text = waveNum.ToString();
        List<GameObject> activeAsteroids = GetAllTagged(player.transform, "asteroid");
        foreach(GameObject active in activeAsteroids)
        {
            Destroy(active);
        }
    }
    private List<GameObject> GetAllTagged(Transform parent, string tag)
    {
        //searches down hierarchy for specific tagged GameObjs
        List<GameObject> arrayOfTagged = new List<GameObject>();
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == tag)
            {
                arrayOfTagged.Add(child.gameObject);
            }
            GetAllTagged(child, tag);
        }
        return arrayOfTagged;
    }
}
