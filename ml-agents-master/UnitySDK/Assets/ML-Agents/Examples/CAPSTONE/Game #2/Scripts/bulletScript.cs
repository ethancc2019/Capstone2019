using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Input;

public class bulletScript : MonoBehaviour
{



    private GameObject player;
    private GameObject asteroid;

    private GameTwoMovement gameTwoScript; //Using this reference to increment the score when the player destorys a asteroid

    public float spped;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        GetComponent<Rigidbody2D>().AddForce(player.transform.up * spped);
        gameTwoScript = GameObject.FindGameObjectWithTag("Player").GetComponent<GameTwoMovement>();

    }



    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("tesd");

        if (other.gameObject.CompareTag("Finish"))
        {
            Destroy(gameObject);

        }
        if (other.gameObject.CompareTag("asteroid"))
        {
            Destroy(GameObject.FindGameObjectWithTag("asteroid"));
            gameTwoScript.score++;
        }
       


    }
}
