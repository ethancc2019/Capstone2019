using UnityEngine;
using MLAgents;
using System.Linq;
using System;
public class Game1Agent : Agent
{
    // Start is called before the first frame update
    private RayPerception3D rayPerception;
    private CharacterController controller;
    private GameObject currentGoal;
    private GameObject floorObj;
    private TextMesh cumulativeRewardText;
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

        CharacterController controller = GetComponent<CharacterController>();
        // is the controller on the ground?
        if (controller.isGrounded)
        {

            //Feed moveDirection with input.
            moveDirection = new Vector3(forward, 0, strafe);
            moveDirection = transform.TransformDirection(moveDirection);
            Debug.Log("Move direction: " + moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;
            //Jumping
            if (jump > 0)
                moveDirection.y = jumpSpeed;

        }
        //Applying gravity to the controller
        moveDirection.y -= gravity * Time.deltaTime;
        //Making the character move
        controller.Move(moveDirection * Time.deltaTime);

        AddReward(-1f / agentParameters.maxStep);
    }

    public override void AgentReset()
    {
        //Score negation, punishment for falling off the edge.
        this.transform.position = spawnPointOne.transform.position;
    }
    public override void CollectObservations()
    {
        //Distance to next goal (needs a get closest goal method maybe?
        //TODO: replace first transform.position with goal position?
        //TODO: increasing difficulty (harder platforms, etc)
        //Distance to goal
        //TODO: Goal position here needs to be replaced.
        AddVectorObs(Vector3.Distance(currentGoal.transform.position, transform.position));
        //Direction to goal
        AddVectorObs((currentGoal.transform.position - transform.position).normalized);
        AddVectorObs(floorObj.transform.position);

        AddVectorObs(floorObj.transform.localScale);
        //Agent's direction
        AddVectorObs(transform.forward);

        //RayPerception (sight)
        //rayDistance: distance of raycasting
        //rayAngles: Angles to raycast (0 is right, 90 is forward, 180 is left
        //detectableObjects: List of tags which correspond to object types the agent can see
        //startOffset: offset from center where to perceive from
        //endOffset: ending offset from where to perceive from

        float rayDistance = 10f;
        float[] rayAngles = {0f, 30f, 60f, 90f, 120f, 150f, 180f};
        string[] detectableObjects = {"wall", "goal", "platform", "floor"};
        //I hypothesize that offsets 0f&0f will need to be changed to be at the level of the ball, should be okay for now.
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
    }
    private void Start()
    {
        //controller = GetComponent<CharacterController>();
        rayPerception = GetComponent<RayPerception3D>();
        currentGoal = GetClosestGoal();

        spawnPointOne = GameObject.Find("SP #1");
        //startPosition.position = this.transform.position;
        floorObj = GameObject.FindGameObjectWithTag("floor");
    }
    private GameObject GetClosestGoal()
    {
        //Returns closest goal, trending in positive z+ axis.
        GameObject[] Goals = GameObject.FindGameObjectsWithTag("goal");
        Goals.OrderBy(
           x => Vector2.Distance(this.transform.position, x.transform.position)
          ).ToList();
        GameObject closestGoal = Goals[Goals.Length-1];
        float shortestDistance = Vector3.Distance(this.transform.position, closestGoal.transform.position);
        foreach (GameObject goal in Goals)
        {
            float distance = Vector3.Distance(this.transform.position, goal.transform.position);
            if(this.transform.position.z < goal.transform.position.z && distance < shortestDistance)
            {
                shortestDistance = distance;
                closestGoal = goal;
            }
        }
        return closestGoal;
    }
    private void FixedUpdate()
    {
        //String rewardStr = String.Format("Reward currently: {0} ", GetCumulativeReward());
        //Debug.Log(rewardStr);
        String value = String.Format("{0}", GetCumulativeReward());
        //cumulativeRewardText.SendMessage(value);
        currentGoal = GetClosestGoal();

        
        //Checks if agent falls off
        if (transform.position.y <= -5)
        {
            AddReward(-0.1f);
            AgentReset();
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("wall"))
        {
            AddReward(-0.05f);
        }
        else if (collision.transform.CompareTag("goal"))
        {
            //TODO: Goal gameObject = GetClosestGoal();
            Debug.Log("Goal Hit!");
            AddReward(1f);
            AgentReset();
        }
    }
}