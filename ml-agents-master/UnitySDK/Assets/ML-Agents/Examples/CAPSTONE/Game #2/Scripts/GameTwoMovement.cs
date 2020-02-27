using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTwoMovement : MonoBehaviour
{

    public float verticalInputAcceleration = 1;
    public float horizontalInputAcceleration = 20;

    

    //Vars for the shooting
    public GameObject bulletPrefab;
    public Transform point;
    public Rigidbody2D rb;


    public Text scoreText;
    public int score = 0;
    public GameObject goal;

    private GameObject spawnPointGameObject;
    private PowerUpSpawnner spawnPointScript;

    //MOVEMENT RE-WORK
    public float speed = 5f;
    private Vector2 movement;
    private Vector2 mousePosition;
    public Camera cam;

    void Start()
    {
        //Camera.main.enabled = true;
        scoreText = GameObject.FindGameObjectWithTag("score_text").GetComponent<Text>();
        spawnPointGameObject = GameObject.FindGameObjectWithTag("spawn_point_container");
        spawnPointScript = spawnPointGameObject.GetComponent<PowerUpSpawnner>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        
        //Mouse rotations
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);


        ////Taking care of keeping the player in the screen bounds
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        
        if (Input.GetButtonDown("Fire1")) //Can make this automatic firing if we want 
        {
            Shoot();

        }

        scoreText.text = score.ToString();


    }


    public void Shoot()
    {
        Debug.Log("Pew Pew!");
        Instantiate(bulletPrefab, point.position, Quaternion.identity);
        //rb = GameObject.Find("Bullet").GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("goal"))
        {
            Debug.Log("Goal hit!");
            this.score++;
            spawnPointScript.DestoryPowerUp();
            //spawnPointScript.activePowerups--;
        }

        if (other.gameObject.CompareTag("asteroid"))
        {
            Debug.Log("Asteroid hit!");
            this.score--;
            //Either kill player here or decrement his score
        }


        //switch (other.gameObject.tag)
        //{
        //    case "goal":
        //        Debug.Log("Goal hit!");
        //        this.score++;
        //        spawnPointScript.DestoryPowerUp();
        //        break;
        //    case "asteroid":
        //        Debug.Log("Player hit by asteroid!");
        //        break;
        //}
    }
}
