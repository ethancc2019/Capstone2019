using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{

    public GameObject gameManager;
    public GameObject player;
    public GameObject spawnPoint;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        gameManager.GetComponent<GameManager>().decreaseScore();
        player.transform.position = spawnPoint.transform.position;
    }
}
