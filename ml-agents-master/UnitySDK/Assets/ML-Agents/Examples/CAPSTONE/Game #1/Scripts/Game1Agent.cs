using UnityEngine;
using MLAgents;
using System.Linq;
using System;
public class Game1Agent : Agent
{
    // Start is called before the first frame update
    private RayPerception3D rayPerception;
    private RayPerception3D floorPerception;
    private CharacterController controller;
    private GameObject currentGoal;
    private GameObject floorObj;
    private TextMesh cumulativeRewardText;
    private GameObject closestFloor;
    private float initialDistanceToGoal;
    private float cumulativeDistance;
    private Vector3 startingPosition;
    public float fallMultiplier = 2.5f;
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private GameObject spawnPointOne;
    private Transform startPosition;
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
        //gets next goal after reset (might not work)
        currentGoal = GetClosestGoal();
        //Debug.Log("My current goal is at: " + currentGoal.transform.position);
        //String rewardStr = String.Format("Reward currently: {0} ", GetCumulativeReward());
        //Debug.Log(rewardStr);
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
        //AddVectorObs(GetComponent<Rigidbody>().velocity);

        //RayPerception (sight)
        //rayDistance: distance of raycasting
        //rayAngles: Angles to raycast (0 is right, 90 is forward, 180 is left
        //detectableObjects: List of tags which correspond to object types the agent can see
        //startOffset: offset from center where to perceive from
        //endOffset: ending offset from where to perceive from

        float rayDistance = 10f;
        float[] rayAngles = { 0f, 30f, 60f, 90f, 120f, 150f, 180f };
        string[] detectableObjects = { "wall", "goal", "platform" };
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));

        float floorDistance = 1f;
        float[] floorAngles = {90f};
        string[] detectableFloors = { "floor", "platform" };
        AddVectorObs(rayPerception.Perceive(floorDistance, floorAngles, detectableFloors, 0f, -2f));
    }
    private void Start()
    {
        //controller = GetComponent<CharacterController>();
        startingPosition = transform.position;
        rayPerception = GetComponent<RayPerception3D>();
        currentGoal = GetClosestGoal();
        cumulativeDistance = Vector3.Distance(startingPosition, currentGoal.transform.position);
        //initialDistanceToGoal = Vector3.Distance(currentGoal.transform.position, transform.position);
    }
    private GameObject GetClosestGoal()
    {
        //Returns closest goal relative to the current floor gameObject that player is standing on
        closestFloor = GetClosestFloor();
        GameObject[] Goals = GameObject.FindGameObjectsWithTag("goal_1");
        Goals = GameObject.FindGameObjectsWithTag("goal_2");
        Goals = GameObject.FindGameObjectsWithTag("goal_3");
        Goals = GameObject.FindGameObjectsWithTag("goal_4");
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
                break;
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
                break;
                
            }
        }
        return closestFloor;
    }
    private void FixedUpdate()
    {
        //cumulativeRewardText.SendMessage(value);
        //Checks if agent falls off
        if (transform.position.y <= closestFloor.transform.position.y - 2)
        {
            //String rewardStr = String.Format("{0}: Reward currently: {1} ", gameObject.name, GetCumulativeReward());
            //Debug.Log(rewardStr);
            //changes reward depending on distance from goal when failed
            //float distanceReward = Mathf.Log(Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance) * 0.2f;
            float distanceReward = Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance;
            AddReward(-1f * distanceReward);
            Debug.Log("Player fell");
            //Score negation, punishment for falling off the edge.
            currentGoal = GetClosestGoal();
            GetComponent<PlayerMovement>().ResetPlayer();
            Done();
            startingPosition = transform.position;
            cumulativeDistance = Vector3.Distance(startingPosition, currentGoal.transform.position);
        }
    }
    //TODO: Urgent refactoring
    private void OnTriggerEnter(Collider collision)
    {
        //hitting a wall gives negative rewards (may change)
        if (collision.transform.CompareTag("wall"))
        {
            //changes reward depending on distance from goal when failed
            //This reward is mysterious but it reduces the negative effects of hitting a wall/falling if it gets to later stages
            //float distanceReward = Mathf.Log(Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance) * 0.2f;

            float distanceReward = Vector3.Distance(transform.position, currentGoal.transform.position) / cumulativeDistance;
            //this is supposed to reduce the negative effects of this punishment for falling off/hitting a wall
            AddReward(-1f);
            //String rewardStr = String.Format("{0}: Reward currently: {1} ", gameObject.name, GetCumulativeReward());
            currentGoal = GetClosestGoal();
            Debug.Log("Wall hit!");
            //Debug.Log(rewardStr);
            GetComponent<PlayerMovement>().ResetPlayer();
            Done();
            startingPosition = transform.position;
            cumulativeDistance = Vector3.Distance(startingPosition, currentGoal.transform.position);
        }
        //hitting a goal is positive!
        else if (collision.transform.CompareTag("goal_1"))
        {
            AddReward(0.3f);
            Debug.Log("Goal 1 hit! 0.3f added");
            //Debug.Log("My current goal was at: " + currentGoal.transform.position);
            cumulativeDistance += Vector3.Distance(startingPosition,currentGoal.transform.position);
            currentGoal = GetClosestGoal();
            //String rewardStr = String.Format("{0}: Reward currently: {1} ", gameObject.name, GetCumulativeReward());
            //Debug.Log(rewardStr);
            Done();
            startingPosition = transform.position;
        }
        else if (collision.transform.CompareTag("goal_2"))
        {
            AddReward(0.5f);
            Debug.Log("Goal 2 hit! 0.5f added");
            //Debug.Log("My current goal was at: " + currentGoal.transform.position);
            cumulativeDistance += Vector3.Distance(startingPosition, currentGoal.transform.position);
            currentGoal = GetClosestGoal();
            //String rewardStr = String.Format("{0}: Reward currently: {1} ", gameObject.name, GetCumulativeReward());
            //Debug.Log(rewardStr);
            Done();
            startingPosition = transform.position;
        }
        else if (collision.transform.CompareTag("goal_3"))
        {
            AddReward(0.8f);
            Debug.Log("Goal 3 hit! 0.8f added");
            //Debug.Log("My current goal was at: " + currentGoal.transform.position);
            cumulativeDistance += Vector3.Distance(startingPosition, currentGoal.transform.position);
            currentGoal = GetClosestGoal();
            //String rewardStr = String.Format("{0}: Reward currently: {1} ", gameObject.name, GetCumulativeReward());
            //Debug.Log(rewardStr);
            Done();
            startingPosition = transform.position;
        }
        else if (collision.transform.CompareTag("goal_4"))
        {
            AddReward(1f);
            Debug.Log("Goal 4 hit! 1f added. Agent has finished game");
            //Debug.Log("My current goal was at: " + currentGoal.transform.position);
            cumulativeDistance += Vector3.Distance(startingPosition, currentGoal.transform.position);
            currentGoal = GetClosestGoal();
            //String rewardStr = String.Format("{0}: Reward currently: {1} ", gameObject.name, GetCumulativeReward());
            //Debug.Log(rewardStr);
            Done();
            startingPosition = transform.position;
        }
        else if (collision.transform.CompareTag("platform"))
        {
            AddReward(0.1f);
            Debug.Log(gameObject.name + "Platform hit.");
        }
    }
}