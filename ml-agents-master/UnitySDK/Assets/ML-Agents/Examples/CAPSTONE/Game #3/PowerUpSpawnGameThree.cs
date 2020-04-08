using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnGameThree : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public GameObject[] powerUpSpawnPoints;

    public int numOfPowerUpsToSpawn;
    public int activePowerUps;

    public List<GameObject> ActivePowerGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        activePowerUps = numOfPowerUpsToSpawn;

        //spawnPowerUps();
    }

    
    public void spawnPowerUps()
    {
        ActivePowerGameObjects.Clear();
        List<int> tempPU = new List<int>();
        int randomIndex = 0;
        for (int i = 0; i < numOfPowerUpsToSpawn; i++)
        {
            RESTART:
            randomIndex = Random.Range(0, powerUpSpawnPoints.Length); //Get random index from the array
            if (!tempPU.Contains(randomIndex))
            {
                tempPU.Add(randomIndex);
                GameObject temp = Instantiate(powerUpPrefab, powerUpSpawnPoints[randomIndex].transform.position, Quaternion.identity);
                temp.transform.parent = this.transform.parent;
                ActivePowerGameObjects.Add(temp);
            }
            else
            {
                goto RESTART;
            }

        }
        activePowerUps = numOfPowerUpsToSpawn;

    }

    public void DestroyAllPowerups()
    {
        if (ActivePowerGameObjects.Count > 0)
        {
            for(int i = 0; i < ActivePowerGameObjects.Count; i++)
            {
                if (ActivePowerGameObjects[i] != null)
                {
                    Destroy(ActivePowerGameObjects[i]);
                    ActivePowerGameObjects.Remove(ActivePowerGameObjects[i]);
                }
                
            }
        }

        
    }
}
