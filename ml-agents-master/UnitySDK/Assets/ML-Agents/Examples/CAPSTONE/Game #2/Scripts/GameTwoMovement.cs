using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTwoMovement : MonoBehaviour {

    public float verticalInputAcceleration = 1;
    public float horizontalInputAcceleration = 20;

    public float maxSpeed = 0.2f;
    public float maxRotationSpeed = 100;
    

    public float velocityDrag = 1;
    public float rotationDrag = 1;

    private Vector3 velocity;
    private float rotationVelocity;

    //Vars for the shooting
    public GameObject bulletPrefab;
    public Transform point;
    public Rigidbody2D rb;
   

    public Text scoreText;
    public int score = 0;
    public GameObject goal;

    private GameObject spawnPointGameObject;
    private PowerUpSpawnner spawnPointScript;


    void Start()
    {
        //Camera.main.enabled = true;
        //scoreText = GameObject.FindGameObjectWithTag("score_text").GetComponent<Text>();
        spawnPointGameObject = GameObject.FindGameObjectWithTag("spawn_point_container");
        spawnPointScript = spawnPointGameObject.GetComponent<PowerUpSpawnner>();
    }

    private void Update()
    {
        // apply forward input
        Vector3 acceleration = Input.GetAxis("Vertical") * verticalInputAcceleration * transform.up;
        velocity += acceleration * Time.deltaTime;

        // apply turn input
        float turnAccleration = -1 * Input.GetAxis("Horizontal") * horizontalInputAcceleration;
        rotationVelocity += turnAccleration * Time.deltaTime;

        ////Taking care of keeping the player in the screen bounds
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
        
        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) //Can make this automatic firing if we want 
        //{
        //    Shoot();
        //}
        if (Input.GetKeyDown(KeyCode.Space)) //Can make this automatic firing if we want 
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
        // apply velocity drag
        velocity = velocity * (1 - Time.deltaTime * velocityDrag);

        // clamp to maxSpeed
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // apply rotation drag
        rotationVelocity = rotationVelocity * (1 - Time.deltaTime * rotationDrag);

        // clamp to maxRotationSpeed
        rotationVelocity = Mathf.Clamp(rotationVelocity, -maxRotationSpeed, maxRotationSpeed);

        // update transform
        transform.position += velocity * Time.deltaTime;
        transform.Rotate(0, 0, rotationVelocity * Time.deltaTime);

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
            Debug.Log("Goal hit!");
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
