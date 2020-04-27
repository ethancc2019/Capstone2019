using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using MLAgents;

public class Game2Agent : Agent
{
    private RayPerception2D rayPerception;

    private float greatestTimeAlive = -1f;
    private float timeAlive = 0f;

    private Vector2[,] trackedAsteroids;
    private GameObject[] bullets = new GameObject[3];
    public GameObject[] asteroids = new GameObject[5];
    private int bulletsShot;
    public int totalAsteroids;

    public Text scoreText;
    public int score = 0;
    public GameObject goal;

    public GameObject asteroidControllerObject;
    private AsteroidController asteroidController;
    public GameObject spawnPointGameObject;
    private PowerUpSpawnner spawnPointScript;

    public GameObject bulletPrefab;
    public Transform point;
    private Rigidbody2D rb;
    public float bulletSpeed = 20f;

    private Vector2 movement;
    private Vector2 mousePosition;
    public Camera cam;
    public static float speed = 0.10f;
    public static float turnSpeed = 5f;
    private float shootTime;
    private float previousX, previousY;

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (vectorAction[3] == 1f && shootTime <= 0)
        {
            shootTime = 0.5f;
            Shoot();
        }
        float angle = transform.rotation.z;
        //WASD Movement
        if (vectorAction[0] == 1f)
        {
            //W
            //movement.y = 1f;
        }
        else if (vectorAction[0] == 2f)
        {
            //S
            //movement.y = -1f;
        }
        else
        {
            //movement.y = 0f;
        }
        if (vectorAction[1] == 1f)
        {
            //A
            //movement.x = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            //D
            //movement.x = 1f;
        }
        else
        {
            //movement.x = 0;
        }
        if (vectorAction[2] == 1f)
        {
            //Rotate CCW
            rb.rotation += 1 * turnSpeed;
        }
        else if (vectorAction[2] == 2f)
        {
            //Rotate CW
            rb.rotation += -1 * turnSpeed;
        }
        Mathf.Clamp(rb.rotation, 0, 360);
        rb.AddForce(movement * speed / 30 + -(rb.velocity * Time.deltaTime / 50));
        Vector3 pos = cam.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = cam.ViewportToWorldPoint(pos);
        AddReward(-1f / agentParameters.maxStep);

    }
    private Vector2[,] GetClosestAsteroids()
    {
        trackedAsteroids = new Vector2[5, 2]; //5 possible asteroids with 2 attributes per asteroid (position, velocity)

        GameObject currentAsteroid = null;
        for (int i = 0; i < asteroids.Length; i++)
        {
            if (asteroids[i] != null)
            {
                currentAsteroid = asteroids[i];
                trackedAsteroids[i, 0] = new Vector2(currentAsteroid.transform.position.x, currentAsteroid.transform.position.y);
                trackedAsteroids[i, 0].x = (trackedAsteroids[i, 0].x - transform.parent.parent.position.x) / 8.5f; //normalized to bounds of canvas
                trackedAsteroids[i, 0].y = (trackedAsteroids[i, 0].y - transform.parent.parent.position.y) / 4.8f; //normalized to bounds of canvas
                trackedAsteroids[i, 1] = new Vector2(currentAsteroid.GetComponent<Rigidbody2D>().velocity.x, currentAsteroid.GetComponent<Rigidbody2D>().velocity.y);
                trackedAsteroids[i, 1].x = trackedAsteroids[i, 1].x / Asteroid.speed; //normalized to asteroid speed given by curricula
                trackedAsteroids[i, 1].y = trackedAsteroids[i, 1].y / Asteroid.speed; //normalized to asteroid speed given by curricula
            }
            else
            {
                trackedAsteroids[i, 0] = new Vector2(-1, -1);
                trackedAsteroids[i, 1] = new Vector2(-1, -1);
                //Debug.Log("Asteroids: " + trackedAsteroids[i,0] + trackedAsteroids[i,1]);
            }
        }
        
        //2 dimensional array of position, velocity of asteroids
        return trackedAsteroids;
    }
    private Vector2[,] GetActiveBullets()
    {
        Vector2[,] bulletObservations = new Vector2[3, 2];

        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i] != null)
            {
                GameObject currentBullet = bullets[i];
                bulletObservations[i, 0] = new Vector2(currentBullet.transform.position.x, currentBullet.transform.position.y);
                bulletObservations[i, 0].x = (bulletObservations[i, 0].x - transform.parent.parent.position.x)/8.5f; // normalized to bound within bullet barriers
                bulletObservations[i, 0].y = (bulletObservations[i, 0].y - transform.parent.parent.position.y)/4.8f; // normalized to bound within bullet barriers
                bulletObservations[i, 1] = new Vector2(currentBullet.GetComponent<Rigidbody2D>().velocity.x, currentBullet.GetComponent<Rigidbody2D>().velocity.y);
                bulletObservations[i, 1].x = bulletObservations[i, 1].x / (bulletSpeed * 2f); // normalized to max speed of bullet
                bulletObservations[i, 1].y = bulletObservations[i, 1].y / (bulletSpeed * 2f); // normalized to max speed of bullet
            }
            else
            {
                bulletObservations[i, 0] = new Vector2(-1, -1);
                bulletObservations[i, 1] = new Vector2(-1, -1);
            }

        }
        //2 dimensional array of position, velocity of asteroids
        return bulletObservations;
    }

    private List<GameObject> GetAllTagged(Transform parent, string tag)
    {
        //searches down hierarchy for specific tagged GameObjs (Does not have to be in play area)
        List<GameObject> arrayOfTagged = new List<GameObject>();
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == tag)
            {
                arrayOfTagged.Add(child.gameObject);
            }
            GetAllTagged(child, tag);
        }
        return arrayOfTagged;
    }

    public float GetAngleFromPlayer(Vector2 from, Vector2 to)
    {
        float angle = Mathf.Tan((to.y - from.y) / (to.x - from.x));
        angle += 90f;
        angle *= 360f;
        return angle%360f;
    }

    public override void CollectObservations()
    {
        //\\=====All observations are normalized from the range of [-1 , +1]=====//\\ 
        //Player Velocity and Current Rotation
        AddVectorObs(rb.velocity / 8.3f); //normalized 8.3f is the max velocity possible

        AddVectorObs(transform.eulerAngles.z / 360f) ;
        
        //Player Position
        AddVectorObs((transform.position.x - transform.parent.parent.position.x) / 8.5f);
        AddVectorObs((transform.position.y - transform.parent.parent.position.y) / 4.8f);

        Vector2[,] incomingAsteroids = GetClosestAsteroids();

        //Asteroid Positions
        AddVectorObs(incomingAsteroids[0, 0]);
        AddVectorObs(incomingAsteroids[1, 0]);
        AddVectorObs(incomingAsteroids[2, 0]);
        AddVectorObs(incomingAsteroids[3, 0]);
        AddVectorObs(incomingAsteroids[4, 0]);
        //Asteroid Velocities
        AddVectorObs(incomingAsteroids[0, 1]);
        AddVectorObs(incomingAsteroids[1, 1]);
        AddVectorObs(incomingAsteroids[2, 1]);
        AddVectorObs(incomingAsteroids[3, 1]);
        AddVectorObs(incomingAsteroids[4, 1]);
        float playerPosX = (transform.position.x - transform.parent.parent.position.x) / 8.5f;
        float playerPosY = (transform.position.y - transform.parent.parent.position.y) / 4.8f;
        //Asteroid Relative Angles

        for (int i = 0; i < asteroids.Length; i++)
        {
            float angle = 0.0f;
            if (asteroids[i] != null)
            { 
                playerPosX = (transform.position.x - transform.parent.parent.position.x) / 8.5f;
                playerPosY = (transform.position.y - transform.parent.parent.position.y) / 4.8f;
                angle = GetAngleFromPlayer(new Vector2(playerPosX,playerPosY), incomingAsteroids[i,0]);
                if (!(angle > 0.0001f && angle < 0.0001f) && transform.rotation.z > (angle - 0.1f) && transform.rotation.z < (angle + 0.1f))
                {
                    asteroids[i].GetComponent<SpriteRenderer>().color = Color.green;
                    
                    //AddReward(0.1f);
                }
                else
                {
                    asteroids[i].GetComponent<SpriteRenderer>().color = Color.red;
                }
                Debug.Log("asteroid at: " + angle + " compared to transform: " + transform.eulerAngles.z);
            }
            
            AddVectorObs(angle);
            //Debug.Log(i + ", Recorded angle: " + angle);
        
        }

        /*Vector2[,] activeBullets = GetActiveBullets();
        //Bullet Positions
        AddVectorObs(activeBullets[0, 0]);
        AddVectorObs(activeBullets[1, 0]);
        AddVectorObs(activeBullets[2, 0]);
        //Bullet Velocities
        AddVectorObs(activeBullets[0, 1]);
        AddVectorObs(activeBullets[1, 1]);
        AddVectorObs(activeBullets[2, 1]);*/
        //Debug.Log("Player: " + this.transform.parent.parent.parent.name + " Bullet: " + activeBullets[0,0] + activeBullets[0,1]);

        //Powerup Position
        AddVectorObs((spawnPointScript.currentPosition.x - transform.parent.parent.position.x) / 8.5f);
        AddVectorObs((spawnPointScript.currentPosition.y - transform.parent.parent.position.y) / 4.8f);
        /*
        //Raycasts
        float[] angles = { 0f, 45f, 60f, 75f, 90f, 105f, 120f, 135f, 180f, 225f, 270f, 315f };
        string[] tags = { "asteroid", "wall", "goal" };
        AddVectorObs(rayPerception.Perceive(10f, angles, tags));
        */

    }

    public override void AgentReset()
    {
        score = 0;
    }

    public void LevelReset()
    {
        asteroidController.ResetAsteroids();
        this.transform.position = transform.parent.transform.position;
    }

    public override void InitializeAgent()
    {
        transform.position = transform.parent.transform.position;
        spawnPointScript = spawnPointGameObject.GetComponent<PowerUpSpawnner>();
        rb = GetComponent<Rigidbody2D>();
        shootTime = 0f;
        rayPerception = GetComponent<RayPerception2D>();
        asteroidController = asteroidControllerObject.GetComponent<AsteroidController>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("goal"))
        {
            this.score++;
            AddReward(1f);
            //sort of a highscore tracker, encourages to do better each iteration
            if (timeAlive >= greatestTimeAlive)
            {
                AddReward(1);
                greatestTimeAlive = timeAlive;
                timeAlive = 0f;
            }
            spawnPointScript.DestoryPowerUp();
            //spawnPointScript.activePowerups--;

        }

        if (collider.gameObject.CompareTag("asteroid"))
        {
            Destroy(collider.gameObject);
            AddReward(-1f);
            if (timeAlive <= greatestTimeAlive)
            {
                AddReward(-1f);
                timeAlive = 0f;
            }
            LevelReset();
            spawnPointScript.DestoryPowerUp();
            Done();
            this.score--;
            //Either kill player here or decrement his score

        }
    }

    public void FixedUpdate()
    {
        shootTime -= Time.fixedDeltaTime;
        Mathf.Clamp(shootTime, 0, 0.5f);
        timeAlive += Time.deltaTime;
    }

    public void Shoot()
    {
        GameObject bulletTemp = Instantiate(bulletPrefab, point.position, point.rotation, transform);
        Rigidbody2D rb = bulletTemp.GetComponent<Rigidbody2D>();
        rb.AddForce(point.up * bulletSpeed, ForceMode2D.Impulse);
        bullets[bulletsShot % 3] = bulletTemp;
        bulletsShot++;
    }

    private float GetSpeed()
    {
        float speed = Mathf.Sqrt(Mathf.Abs(transform.position.x - previousX) + Mathf.Abs(transform.position.y - previousY));
        previousX = transform.position.x;
        previousY = transform.position.y;
        return speed;
    }

}
