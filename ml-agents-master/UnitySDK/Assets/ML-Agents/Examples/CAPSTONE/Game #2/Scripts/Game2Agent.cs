using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using MLAgents;

public class Game2Agent : Agent
{
    private RayPerception2D rayPerception;

    public Text scoreText;
    public int score = 0;
    public GameObject goal;

    private GameObject spawnPointGameObject;
    private PowerUpSpawnner spawnPointScript;

    public GameObject bulletPrefab;
    public Transform point;
    private Rigidbody2D rb;
    private bulletScript shooting;

    private Vector2 movement;
    private Vector2 mousePosition;
    public Camera cam;
    public static float speed = 5f;
    public static float turnSpeed = 10f;
    private float shootTime;

    public override void AgentAction(float [] vectorAction, string textAction)
    {
        if (vectorAction[3] == 1f && shootTime <= 0)
        {
            shootTime = 0.5f;
            shooting.Shoot();
        }
        float angle = transform.rotation.z;
        //WASD Movement
        if (vectorAction[0] == 1f)
        {
            //W
            movement.y = 1f;
        }
        else if (vectorAction[0] == 2f)
        {
            //S
            movement.y = -1f;
        }
        else
        {
            movement.y = 0f;
        }
        if (vectorAction[1] == 1f)
        {
            //A
            movement.x = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            //D
            movement.x = 1f;
        }
        else
        {
            movement.x = 0;
        }
        if(vectorAction[2] == 1f)
        {
            //Rotate CCW
            rb.rotation += 1 * turnSpeed;
        }
        else if(vectorAction[2] == 2f)
        {
            //Rotate CW
            rb.rotation += -1 * turnSpeed;
        }
        Mathf.Clamp(rb.rotation, 0, 360);
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    public override void CollectObservations()
    {
        AddVectorObs(this.transform.position);
        AddVectorObs(rb.velocity);

        //Position of goal
        AddVectorObs(spawnPointScript.currentPosition);

        //float[] rayAngles = { 0f, 22.5f, 45f, 67.5f, 90f, 112.5f, 135f, 157.5f, 180f, 202.5f, 225f, 247.5f, 270f, 292.5f, 315f, 337.5f };
        float[] angles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
        string[] tags = { "asteroid", "wall" };
        AddVectorObs(rayPerception.Perceive(7f, angles, tags));
    }

    public override void AgentReset()
    {


    }

    public override void InitializeAgent()
    {
        scoreText = GameObject.FindGameObjectWithTag("score_text").GetComponent<Text>();
        spawnPointGameObject = GameObject.FindGameObjectWithTag("spawn_point_container");
        spawnPointScript = spawnPointGameObject.GetComponent<PowerUpSpawnner>();
        rb = GetComponent<Rigidbody2D>();
        shooting = GetComponent<bulletScript>();
        shootTime = 0f;
        rayPerception = GetComponent<RayPerception2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("goal"))
        {
            Debug.Log("Goal hit!");
            this.score++;
            AddReward(1f);
            spawnPointScript.DestoryPowerUp();
            //spawnPointScript.activePowerups--;
        }

        if (other.gameObject.CompareTag("asteroid"))
        {
            AddReward(-1f);
            Debug.Log("Asteroid hit!");
            this.score--;
            //Either kill player here or decrement his score
        }
    }

    public void FixedUpdate()
    {
        shootTime -= Time.fixedDeltaTime;
        Mathf.Clamp(shootTime, 0, 0.5f);

    }
}
