using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{


    public GameObject[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NewSpawnPoint()
    {
        int random = Random.Range(0, spawnPoints.Length - 1);

    }
}
