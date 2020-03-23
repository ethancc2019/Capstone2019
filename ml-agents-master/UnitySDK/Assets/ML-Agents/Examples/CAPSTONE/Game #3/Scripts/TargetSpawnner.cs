using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawnner : MonoBehaviour
{

    public GameObject[] targetSpawnPoints;
    public GameObject TargetPrefabGameObject;
    public int maxNumOfTargetsToSpawn;
    public int activeTargets;

    // Start is called before the first frame update
    void Start()
    {
        activeTargets = maxNumOfTargetsToSpawn;
        spawnTargets();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTargets <= 0)
        {
            Debug.Log("All targets hit!");
        }
    }

    public void spawnTargets()
    {
        int randomIndex = 0;
        for (int i = 0; i < maxNumOfTargetsToSpawn; i++)
        {
            randomIndex = Random.Range(0, targetSpawnPoints.Length); //Get random index from the array
            GameObject temp = Instantiate(TargetPrefabGameObject,targetSpawnPoints[randomIndex].transform.position,Quaternion.identity);
        }
    }
}
