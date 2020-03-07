using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Input;

public class bulletScript : MonoBehaviour
{
    public Transform firePoint;

    public GameObject bulletPrefab;

    public float bulletSpeed = 20f;

    public void Shoot()
    {
        GameObject bulletTemp = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bulletTemp.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "asteroid")
        {
            Destroy(collider.gameObject);
        }

        else if(collider.tag == "Finish")
        {
            Destroy(gameObject);
            Debug.Log("Destroy Bullet");
        }
    }
}
