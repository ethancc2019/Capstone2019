using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{

    //public GameObject gameManager;
    public GameObject player;
    public Game1Area area;
    private GameObject spawnPointOne;



    // Use this for initialization
    void Start ()
    {
        spawnPointOne = area.getSpawnPointOne();
	}
    private void OnTriggerEnter(Collider other)
    {
        //gameManager.GetComponent<GameManager>().decreaseScore();
        player.transform.position = spawnPointOne.transform.position;
    }
}
