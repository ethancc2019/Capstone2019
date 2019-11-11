using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{


    public float velocityX = 0f;
    private float velocityY = 5f;

    private Rigidbody2D rb;

    public GameObject bullet;

    public Transform point;
	// Use this for initialization
	void Start ()
	{
	    rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        Instantiate(bullet, point.position, Quaternion.identity);
            rb.velocity = new Vector2(velocityX, velocityY);

        }
    }
}
