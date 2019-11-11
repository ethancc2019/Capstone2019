using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{



    private GameObject player;

    public float spped;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        GetComponent<Rigidbody2D>().AddForce(player.transform.up * spped);

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("tesd");
        if (other.gameObject.tag == "Finish")
        {
            Destroy(gameObject);   
        }
    }
}
