using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDetection : MonoBehaviour
{

    public GameObject bullet;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter2d(Collider other)
    {
        Debug.Log("Hit the boundary");

        if (other.tag == "Finish")
        {
            Destroy(gameObject);
        }

    }
}
