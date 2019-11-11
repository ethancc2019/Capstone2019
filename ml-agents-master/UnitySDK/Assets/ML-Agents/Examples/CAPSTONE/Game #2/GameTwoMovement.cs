using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTwoMovement : MonoBehaviour {

    public float verticalInputAcceleration = 1;
    public float horizontalInputAcceleration = 20;

    public float maxSpeed = 10;
    public float maxRotationSpeed = 100;

    public float velocityDrag = 1;
    public float rotationDrag = 1;

    private Vector3 velocity;
    private float rotationVelocity;

    //Vars for the shooting
    public GameObject bulletPrefab;
    public Transform point;
    public Rigidbody2D rb;
   

    private Text scoreText;
    private int score = 0;
    public GameObject goal;
    void Start()
    {
        //Camera.main.enabled = true;
    }

    private void Update()
    {
        // apply forward input
        Vector3 acceleration = Input.GetAxis("Vertical") * verticalInputAcceleration * transform.up;
        velocity += acceleration * Time.deltaTime;

        // apply turn input
        float turnAccleration = -1 * Input.GetAxis("Horizontal") * horizontalInputAcceleration;
        rotationVelocity += turnAccleration * Time.deltaTime;

        //Taking care of keeping th e player in the screen bounds
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        if (Input.GetKeyDown(KeyCode.Space)) //Can make this automatic firing if we want 
        {
            Shoot();
            

        }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "goal")
        {
            this.score++;
            Destroy(goal);
        }
    }
}
