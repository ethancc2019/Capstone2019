using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseScore : MonoBehaviour {

    public int score = 0;
    public GameObject player;
    public GameObject spawnPoint;


    private void OnTriggerEnter(Collider other)
    {
        score++;
        Debug.Log(score.ToString());
        player.transform.position = spawnPoint.transform.position;
    }
}
