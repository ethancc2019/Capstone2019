using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System.Linq;
using System;
using TMPro;
public class Game1Agent : Agent
{
    // Start is called before the first frame update
    public Material current;
    public Material normal;
    public string agentNum;
    public Game1Area game1Area;
    public GameObject levelPosition;
    public GameObject inactivePosition;
    private List<GameObject> spawns;
    private List<GameObject> goals;
    private List<GameObject> stages;
    private RayPerception3D rayPerception;
    private RayPerception3D rayPerceptionFloor;
    private RayPerception3D rayPerceptionCeiling;
    private CharacterController controller;
    private GameObject currentGoal;
    private GameObject currentStage;
    private TextMesh cumulativeRewardText;
    private GameObject closestFloor;
    private float initialDistanceToGoal;
    private float newDistance;
    private float cumulativeDistance;
    private short jumpCount;
    private GameObject goalMarker;
    private GameObject spawnMarker;
    private Boolean wallDetection;
    private Vector3 startingPosition;
    public GameObject spawnMarkerPrefab;
    public GameObject goalMarkerPrefab;
    public float fallMultiplier = 1.5f;
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public float rotationSpeed = 1f;
    private Vector3 moveDirection = Vector3.zero;
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float forward = 0f;
        float strafe = 0f;
        float rotate = 0f;
        float jump = vectorAction[2];

        //WASD Movement
        //vectorAction[0] will be forward/backward movement.
        if (vectorAction[0] == 1f)
        {
            //agent decides to move backwards
            forward = 1f;
        }
        else if (vectorAction[0] == 2f)
        {
            //agent decides to move backwards
            forward = -1f;
        }
        if (vectorAction[1] == 1f)
        {
            //agent decides to move left
            strafe = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            //agent decides to move right
            strafe = 1f;
        }
        if (vectorAction[3] == 1f)
        {
            //agent decides to move left
            rotate = -1f;
        }
        else if (vectorAction[3] == 2f)
        {
            //agent decides to move right
            rotate = 1f;
        }
        //if(vectorAction[2] > 0)
        //{
        //AddReward(-2f / agentParameters.maxStep);
        //Debug.Log("jumping: " + vectorAction[2]);
        // }

        // is the controller on the ground?
        if (controller.isGrounded)
        {
            speed = 6.0F;
            //Feed moveDirection with input.
            moveDirection = new Vector3(strafe, 0, forward);
            //Debug.Log("Move direction: " + moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;
            //Jumping
            if (jump > 0)
            {
                moveDirection.y = jumpSpeed;
                jumpCount++;
            }
        }
        else
        {
            speed = 4.0F;
            moveDirection = new Vector3(strafe, moveDirection.y, forward);
            moveDirection.x *= speed;
            moveDirection.z *= speed;
        }
        //Applying gravity to the controller
        moveDirection.y -= (-Physics2D.gravity.y) * (fallMultiplier) * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        transform.Rotate(0, rotate * rotationSpeed, 0);
        newDistance = initialDistanceToGoal;
        initialDistanceToGoal = Vector3.Distance(transform.position, currentGoal.transform.position);
        if (initialDistanceToGoal < 1.0f)
        {
            Debug.Log("Goal Hit");
            AddReward(1);
            Done();
            LevelReset();
        }

        //Checks if agent falls off
        if (transform.position.y < closestFloor.transform.position.y)
        {
            //changes reward depending on distance from goal when failed
            //float distanceReward = Mathf.Log(Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance) * 0.2f;
            float distanceReward = Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance;
            AddReward(-1f);
            //Score negation, punishment for falling off the edge.  
            GetComponent<PlayerMovement>().ResetPlayer();
            Done();
            LevelReset();
            cumulativeDistance = Vector3.Distance(startingPosition, currentGoal.transform.position);
        }
        AddReward(-1f / agentParameters.maxStep);
    }

    public override void AgentReset()
    {
        closestFloor = GetClosestFloor();
    }
    public void LevelReset()
    {
        currentStage.transform.position = inactivePosition.transform.position;
        int index = UnityEngine.Random.Range(0, goals.Count());
        currentStage = stages[index];
        currentStage.transform.position = levelPosition.transform.position;
        currentGoal = goals[index];
        currentGoal.GetComponent<MoveGoal>().Move();
        spawnMarker = spawns[index];
        transform.position = spawnMarker.transform.position;
        closestFloor = GetClosestFloor();
        initialDistanceToGoal = Vector3.Distance(currentGoal.transform.position, transform.position);
        jumpCount = 0;
        List<GameObject> walls = new List<GameObject>();
        walls = GetWalls();
        foreach (GameObject wall in walls)
        {
            if (currentGoal.GetComponent<MeshRenderer>().bounds.Intersects(wall.GetComponent<MeshRenderer>().bounds))
            {
                //Debug.Log("Goal intersects walls");
                LevelReset();
                break;
            }
        }
        moveDirection = Vector3.zero;
    }
    public List<GameObject> GetWalls()
    {
        GameObject agentArena = GameObject.Find("SimpleLevels " + agentNum);
        List<GameObject> wallList = new List<GameObject>();
        foreach (Transform walls in currentStage.transform)
        {
            if (walls.name == "Walls")
            {
                foreach (Transform wall in walls)
                {
                    if (wall.tag == "wall")
                    {
                        wallList.Add(wall.gameObject);
                    }
                }
            }
            else if (walls.tag == "wall")
            {
                wallList.Add(walls.gameObject);
            }
        }
        return wallList;
    }
    public override void CollectObservations()
    {
        //Distance to next goal (needs a get closest goal method maybe?
        //TODO: increasing difficulty (harder platforms, etc)
        //Distance to goal
        AddVectorObs(Vector3.Distance(currentGoal.transform.position, transform.position));
        //Direction to goal
        //AddVectorObs((currentGoal.transform.position - transform.position).normalized);
        AddVectorObs(currentGoal.transform.position);
        //Agent's direction
        AddVectorObs(transform.forward);

        //TODO: Floor Detection
        //center position of floor
        //AddVectorObs(closestFloor.transform.position);
        //current floor dimensions based on what the Collider component for the gameObject contains
        //AddVectorObs(closestFloor.GetComponent<Collider>().bounds.size);
        //compare x pos to left edge of floor
        //float playerX = transform.position.x;
        //float playerZ = transform.position.z;
        //float floorX = closestFloor.transform.position.x;
        //float floorZ = closestFloor.transform.position.z;
        //float floorSizeX = closestFloor.GetComponent<Collider>().bounds.size.x;
        //float floorSizeZ = closestFloor.GetComponent<Collider>().bounds.size.z;
        //AddVectorObs(playerX - floorSizeX/2);
        ////compare x pos to right edge of floor
        //AddVectorObs(playerX + floorSizeX/2);
        ////compare z pos to top edge of floor
        //AddVectorObs(playerZ - floorSizeZ/2);
        ////compare z pos to bottom edge of floor
        //AddVectorObs(playerZ + floorSizeZ/2);

        //Player position
        AddVectorObs(this.transform.position);
        AddVectorObs(controller.velocity);


        //player's velocity
        //AddVectorObs(GetComponent<Rigidbody>().velocity);

        //RayPerception (sight)
        //rayDistance: distance of raycasting
        //rayAngles: Angles to raycast (0 is right, 90 is forward, 180 is left
        //detectableObjects: List of tags which correspond to object types the agent can see
        //startOffset: offset from center where to perceive from
        //endOffset: ending offset from where to perceive from

        float rayDistance = 20f;
        float[] rayAngles = {45f, 60f, 75f, 90f, 105f, 120f, 135f};
        string[] detectableObjects = { "wall", "goal", "platform", "floor" };
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));

        float floorDistance = 20f;
        float[] floorAngles = { 75f, 82.5f, 90f, 97.5f, 105f };
        string[] detectableFloors = { "floor", "platform", "goal", "wall" };
        AddVectorObs(rayPerceptionFloor.Perceive(floorDistance, floorAngles, detectableFloors, 0f, -4f));

        float ceilingDistance = 20f;
        float[] ceilingAngles = {75f, 82.5f, 90f, 97.5f, 105f };
        string[] detectableCeilings = { "floor", "platform", "goal", "wall" };
        AddVectorObs(rayPerceptionCeiling.Perceive(ceilingDistance, ceilingAngles, detectableCeilings, 0f, 4f));
    }
    public override void InitializeAgent()
    {
        //controller = GetComponent<CharacterController>();
        startingPosition = transform.position;
        rayPerception = GetComponent<RayPerception3D>();
        rayPerceptionFloor = GetComponent<RayPerception3D>();
        rayPerceptionCeiling = GetComponent<RayPerception3D>();

        controller = GetComponent<CharacterController>();

        spawns = new List<GameObject>();
        goals = new List<GameObject>();
        stages = new List<GameObject>();
        GameObject agentArena = GameObject.Find("SimpleLevels " + agentNum);
        foreach(Transform stage in agentArena.transform)
        {
            if(stage.tag == "stage")
            {
                stages.Add(stage.gameObject);
                foreach (Transform spawnGoal in stage)
                {
                    //Could just do index 0 and 1 but we would need to always make sure its goal then spawn and no other objects

                    if (spawnGoal.tag == "spawnGoal")
                    {
                        foreach (Transform child in spawnGoal)
                        {
                            if (child.tag == "goal")
                            {
                                goals.Add(child.gameObject);
                            }
                            else if (child.tag == "spawn")
                            {
                                spawns.Add(child.gameObject);
                            }
                        }
                    }
                }
            }
            
        }
        jumpCount = 0;
        currentStage = stages[0];
        currentStage.transform.position = levelPosition.transform.position;
        currentGoal = goals[0];
        cumulativeDistance = Vector3.Distance(startingPosition, currentGoal.transform.position);
        spawnMarker = spawns[0];
        //CreateMarkers();
        transform.position = spawnMarker.transform.position;
        closestFloor = GetClosestFloor();
        initialDistanceToGoal = Vector3.Distance(currentGoal.transform.position, transform.position);
        wallDetection = false;
    }
    public void CreateMarkers()
    {
        Destroy(goalMarker);
        goalMarker = Instantiate<GameObject>(goalMarkerPrefab.gameObject);
        goalMarker.transform.position = currentGoal.transform.position + new Vector3(0,2,0);
        Destroy(spawnMarker);
        spawnMarker = Instantiate<GameObject>(spawnMarkerPrefab.gameObject);
        spawnMarker.transform.position = startingPosition + new Vector3(0, 2, 0);
    }
    private GameObject GetClosestGoal()
    {
        //Returns closest goal relative to the current floor gameObject that player is standing on
        closestFloor = GetClosestFloor();
        GameObject[] Goals = GameObject.FindGameObjectsWithTag("goal");
        GameObject closestGoal = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject goal in Goals)
        {
            //again, it compares the distance of the goal from the current floor gameObject.
            float distanceFromFloor = Vector3.Distance(closestFloor.transform.position, goal.transform.position);
            if (distanceFromFloor < shortestDistance)
            {
                shortestDistance = distanceFromFloor;
                closestGoal = goal;
            }
        }
        return closestGoal;
    }
    private GameObject GetClosestFloor()
    {
        //Gets nearest floor object player is on.
        GameObject[] floors = GameObject.FindGameObjectsWithTag("floor");
        GameObject closestFloor = null; //returns null if no floors are found
        float shortestDistance = Mathf.Infinity;
        foreach (GameObject floor in floors)
        {
            float distance = Vector3.Distance(this.transform.position, floor.transform.position);
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                closestFloor = floor;
                
            }
        }
        //closestFloor.GetComponent<MeshRenderer>().material = this.current;
        return closestFloor;
    }
    //TODO: Urgent refactoring
    private void OnTriggerEnter(Collider collision)
    {
        //hitting a wall gives some negative rewards 
        if (collision.transform.CompareTag("wall"))
        {
            ////changes reward depending on distance from goal when failed
            ////This reward is mysterious but it reduces the negative effects of hitting a wall/falling if it gets to later stages
            ////float distanceReward = Mathf.Log(Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance) * 0.2f;
            //float distanceReward = Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance;
            ////this is supposed to reduce the negative effects of this punishment for falling off/hitting a wall
            //AddReward(-1f);
            //Debug.Log("Wall hit!");
            //AgentReset();
            //cumulativeDistance += Vector3.Distance(startingPosition, currentGoal.transform.position);
            AddReward(-1);
            Done();
            LevelReset();
        }
        //hitting a goal is good!
        else if (collision.transform.CompareTag("goal"))
        {

        }
        //hitting a platform is good
        else if (collision.transform.CompareTag("platform"))
        {
            Debug.Log(gameObject.name + "Platform hit.");
        }
    }
    public GameObject GetCurrentGoal()
    {
        return this.currentGoal;
    }
}