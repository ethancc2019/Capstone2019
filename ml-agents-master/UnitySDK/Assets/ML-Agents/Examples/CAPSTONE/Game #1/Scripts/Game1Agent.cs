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
    public Game1Area game1Area;
    private List<GameObject> spawns;
    private List<GameObject> goals;
    private RayPerception3D rayPerception;
    private RayPerception3D floorPerception;
    private CharacterController controller;
    private GameObject currentGoal;
    private TextMesh cumulativeRewardText;
    private GameObject closestFloor;
    private float initialDistanceToGoal;
    private float cumulativeDistance;
    private GameObject goalMarker;
    private GameObject spawnMarker;
    private Vector3 startingPosition;
    public GameObject spawnMarkerPrefab;
    public GameObject goalMarkerPrefab;
    public float fallMultiplier = 2.5f;
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float forward = 0f;
        float strafe = 0f;
        float jump = vectorAction[2];

        //WASD Movement
        //vectorAction[0] will be forward/backward movement.
        if (vectorAction[0] == 1f)
        {
            //agent decides to move backwards
            forward = -1f;
        }
        else if (vectorAction[0] == 2f)
        {
            //agent decides to move backwards
            forward = 1f;
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

        //if(vectorAction[2] > 0)
        //{
            //AddReward(-2f / agentParameters.maxStep);
            //Debug.Log("jumping: " + vectorAction[2]);
       // }

        CharacterController controller = GetComponent<CharacterController>();
        // is the controller on the ground?
        if (controller.isGrounded)
        {

            //Feed moveDirection with input.
            moveDirection = new Vector3(forward, 0, strafe);
            moveDirection = transform.TransformDirection(moveDirection);
            //Debug.Log("Move direction: " + moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;
            //Jumping
            if (jump > 0)
                moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection = new Vector3(forward, moveDirection.y, strafe);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= speed;
            moveDirection.z *= speed;
        }
        //Applying gravity to the controller
        moveDirection.y -= (-Physics2D.gravity.y) * (fallMultiplier - 1) * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        AddReward(-1f / agentParameters.maxStep);
    }

    public override void AgentReset()
    {
        game1Area.ResetArea();
        int index = UnityEngine.Random.Range(0, goals.Count()-1);

        currentGoal = goals[index];
        spawnMarker = spawns[index];
        CreateMarkers();

    }
    public override void CollectObservations()
    {
        //Distance to next goal (needs a get closest goal method maybe?
        //TODO: increasing difficulty (harder platforms, etc)
        //Distance to goal
        AddVectorObs(Vector3.Distance(currentGoal.transform.position, transform.position));
        //Direction to goal
        AddVectorObs((currentGoal.transform.position - transform.position).normalized);
        //Agent's direction
        AddVectorObs(transform.forward);

        //TODO: Floor Detection
        //center position of floor
        AddVectorObs(closestFloor.transform.position);
        //current floor dimensions based on what the Collider component for the gameObject contains
        AddVectorObs(closestFloor.GetComponent<Collider>().bounds.size);
        //compare x pos to left edge of floor
        float playerX = transform.position.x;
        float playerZ = transform.position.z;
        float floorX = closestFloor.transform.position.x;
        float floorZ = closestFloor.transform.position.z;
        float floorSizeX = closestFloor.GetComponent<Collider>().bounds.size.x;
        float floorSizeZ = closestFloor.GetComponent<Collider>().bounds.size.z;
        AddVectorObs(playerX - floorSizeX/2);
        //compare x pos to right edge of floor
        AddVectorObs(playerX + floorSizeX/2);
        //compare z pos to top edge of floor
        AddVectorObs(playerZ - floorSizeZ/2);
        //compare z pos to bottom edge of floor
        AddVectorObs(playerZ + floorSizeZ/2);

        //player's velocity
        AddVectorObs(GetComponent<Rigidbody>().velocity);

        //RayPerception (sight)
        //rayDistance: distance of raycasting
        //rayAngles: Angles to raycast (0 is right, 90 is forward, 180 is left
        //detectableObjects: List of tags which correspond to object types the agent can see
        //startOffset: offset from center where to perceive from
        //endOffset: ending offset from where to perceive from

        float rayDistance = 10f;
        float[] rayAngles = { 0f, 22.5f, 45f, 67.5f, 90f, 112.5f, 135f, 157.5f,180f, 202.5f,225f, 247.5f, 270f, 292.5f, 315f, 337.5f };
        string[] detectableObjects = { "wall", "goal", "platform" };
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));

        float floorDistance = 3f;
        float[] floorAngles = {0f,45f,90f,135f,180f,225f,270f,315f};
        string[] detectableFloors = { "floor", "platform" };
        AddVectorObs(rayPerception.Perceive(floorDistance, floorAngles, detectableFloors, 0f, -2f));
    }
    private void Start()
    {
        //controller = GetComponent<CharacterController>();
        startingPosition = transform.position;
        rayPerception = GetComponent<RayPerception3D>();
        cumulativeDistance = Vector3.Distance(startingPosition, currentGoal.transform.position);
        GameObject[] spawnGoals = GameObject.FindGameObjectsWithTag("spawnGoal");
        spawns = new List<GameObject>();
        goals = new List<GameObject>();
        foreach (GameObject spawnGoal in spawnGoals)
        {
            //Could just do index 0 and 1 but we would need to always make sure its goal then spawn and no other objects
            int index = 0;
            GameObject gameObject;
            while((gameObject=spawnGoal.transform.GetChild(index).gameObject) != null)
            {
                if (transform.tag == "goal")
                {
                    goals.Add(gameObject);
                }
                else if (transform.tag == "spawn")
                {
                    goals.Add(gameObject);
                }
                index++;
            }
            
        }
        currentGoal = goals[0];
        spawnMarker = spawns[0];
        CreateMarkers();
        //initialDistanceToGoal = Vector3.Distance(currentGoal.transform.position, transform.position);
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
        return closestFloor;
    }
    private void FixedUpdate()
    {
        //Checks if agent falls off
        if (transform.localPosition.y <= closestFloor.transform.localPosition.y - 2)
        {
            //changes reward depending on distance from goal when failed
            //float distanceReward = Mathf.Log(Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance) * 0.2f;
            float distanceReward = Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance;
            AddReward(-1f * distanceReward - 0.25f);
            //Score negation, punishment for falling off the edge.  
            GetComponent<PlayerMovement>().ResetPlayer();
            AgentReset();
            cumulativeDistance = Vector3.Distance(startingPosition, currentGoal.transform.position);
        }
    }
    //TODO: Urgent refactoring
    private void OnTriggerEnter(Collider collision)
    {
        //hitting a wall gives some negative rewards 
        if (collision.transform.CompareTag("wall"))
        {
            //changes reward depending on distance from goal when failed
            //This reward is mysterious but it reduces the negative effects of hitting a wall/falling if it gets to later stages
            //float distanceReward = Mathf.Log(Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance) * 0.2f;
            float distanceReward = Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance;
            //this is supposed to reduce the negative effects of this punishment for falling off/hitting a wall
            AddReward(-1f * distanceReward - 0.25f);
            Debug.Log("Wall hit!");
            GetComponent<PlayerMovement>().ResetPlayer();
            AgentReset();
            cumulativeDistance = Vector3.Distance(startingPosition, currentGoal.transform.position);
        }
        //hitting a goal is good!
        else if (collision.transform.CompareTag("goal"))
        {
            AddReward(1f);
            Debug.Log("Goal hit! 0.5f added");
            String rewardStr = String.Format("{0}: Reward currently: {1} ", gameObject.name, GetCumulativeReward());
            Debug.Log(rewardStr);
            //Debug.Log("My current goal was at: " + currentGoal.transform.position);
            cumulativeDistance += Vector3.Distance(startingPosition,currentGoal.transform.position);
            AgentReset();
        }
        //hitting a platform is good
        else if (collision.transform.CompareTag("platform"))
        {
            AddReward(0.1f);
            Debug.Log(gameObject.name + "Platform hit.");
        }
    }
}