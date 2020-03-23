using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public float destroyTime = 0.5f;
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect,destroyTime); //Destroy after 0.5 seconds
        Destroy(gameObject);

        if (collision.collider.CompareTag("barrier"))
        {
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Player_2"))
        {
            Debug.Log("Hit another Player!");
        }
        
    }

}
