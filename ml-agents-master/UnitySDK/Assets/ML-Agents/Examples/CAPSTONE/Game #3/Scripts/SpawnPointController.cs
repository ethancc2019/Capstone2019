using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{

    public GameObject player;
    public GameObject[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayerInRandomPoint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayerInRandomPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        player.transform.position = spawnPoints[randomIndex].transform.position;

    }
}
