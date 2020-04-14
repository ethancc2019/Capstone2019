using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public int ammoCount = 100;
    public Transform firePoint;

    public GameObject bulletPrefab;

    public float bulletSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && ammoCount > 0) //Left click
        {
            Shoot();
            ammoCount--;
           // Debug.Log(ammoCount.ToString());
        }
    }

    void Shoot()
    {
        GameObject bulletTemp = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation) as GameObject;
        bulletTemp.transform.parent = this.transform.parent;
        Rigidbody2D rb = bulletTemp.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);

    }
}
