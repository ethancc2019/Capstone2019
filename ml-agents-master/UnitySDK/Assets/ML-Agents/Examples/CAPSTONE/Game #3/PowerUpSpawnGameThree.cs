using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnGameThree : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public GameObject[] powerUpSpawnPoints;

    public int numOfPowerUpsToSpawn;

    public int activePowerUps; 
    // Start is called before the first frame update
    void Start()
    {
        activePowerUps = numOfPowerUpsToSpawn;

        spawnPowerUps();
    }

    // Update is called once per frame
    void Update()
    {
        if (activePowerUps == 0)
        {
            spawnPowerUps();
            activePowerUps = numOfPowerUpsToSpawn;
            
        }
    }
    public void spawnPowerUps()
    {

        int randomIndex = 0;
        for (int i = 0; i < numOfPowerUpsToSpawn; i++)
        {
            randomIndex = Random.Range(0, powerUpSpawnPoints.Length); //Get random index from the array
            GameObject temp = Instantiate(powerUpPrefab, powerUpSpawnPoints[randomIndex].transform.position, Quaternion.identity);
        }
    }
}
