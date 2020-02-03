using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnner : MonoBehaviour
{


    public GameObject[] spawnPoints;
    public GameObject powerUpPrefab;

	private GameObject tempPowerup;
    public int activePowerups;
	// Use this for initialization
	void Start ()
	{
	    activePowerups = 0;
        spawnPowerUp();
	}
	
	// Update is called once per frame
	void Update () {
	    if (activePowerups <= 0)
	    {
	       // activePowerups--;

            spawnPowerUp();
	    }
	}

    //Debug power up spawn bug
    public void spawnPowerUp()
    {
        int randomPos = Random.Range(0, spawnPoints.Length - 1);
		tempPowerup = Instantiate(powerUpPrefab, spawnPoints[randomPos].transform.position,Quaternion.identity,gameObject.transform);
        activePowerups++;
    }

	public void DestoryPowerUp()
    {
		Destroy(tempPowerup);
		activePowerups--;
    }


}
