using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseScore : MonoBehaviour {

    public GameObject spawnPoint;
    public GameObject player;
    public GameObject gameManager;

    void Start()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        gameManager.GetComponent<GameManager>().increaseScore();
        player.transform.position = spawnPoint.transform.position;
    }
}
