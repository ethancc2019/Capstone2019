using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class Game1Agent : Agent
{
    // Start is called before the first frame update
    private RayPerception3D rayPerception;
    private CharacterController controller;
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


        AddReward(-1f / agentParameters.maxStep);
    }

    public override void AgentReset()
    {
        //Score negation, punishment for falling off the edge.
        GetComponent<PlayerMovement>().ResetPlayer();
    }

    public override void CollectObservations()
    {
        //Distance to next goal (needs a get closest goal method maybe?

        //TODO: how does it find the next goal?
        //TODO: replace first transform.position with goal position?
        //TODO: increasing difficulty (harder platforms, etc)
        //Distance to goal
        AddVectorObs(Vector3.Distance(transform.position, transform.position));
        //Direction to goal
        AddVectorObs((transform.position - transform.position).normalized);
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
        string[] detectableObjects = {"wall", "goal", "platform"};
        //I hypothesize that offsets 0f&0f will need to be changed to be at the level of the ball, should be okay for now.
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rayPerception = GetComponent<RayPerception3D>();
    }

    private void FixedUpdate()
    {
        //Checks if agent falls off
        if (transform.position.y <= -1)
        {
            AddReward(-1f);
            AgentReset();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("wall"))
        {
            AddReward(-0.1f);
        }
        else if (collision.transform.CompareTag("goal"))
        {
            AddReward(2f);
        }
    }
}