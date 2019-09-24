using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class TemplateAgent : Agent
{

    public float speed = 3f;
    public float DistanceToEarnReward = 0.1f;
    public float OnMarkerPoints = 0.1f;

    public GameObject player;
    public Transform startPosition;
    public Rigidbody playerRb;
    public GameObject goal;
    
    public override void CollectObservations()
    {
        AddVectorObs(player.transform.position - goal.transform.position); // distance to target

    }

    public override void AgentAction(float[] vectorAction, string textAction)
	{
	    if (brain.brainParameters.vectorActionSpaceType == SpaceType.discrete)
	    {
	        float action_x = vectorAction[0]; // The agent has only one possible action. Up/Down amount
	        action_x = Mathf.Clamp(action_x, -1, 1); // Bound the action input from -1 to 1
	        action_x = action_x * speed; // Scale in put to marker speed
	        player.transform.position += new Vector3(action_x, 0, 0); // Move up or down

	        if (Mathf.Abs(goal.transform.position.x - player.transform.position.x) <= DistanceToEarnReward)
	        {
	            SetReward(OnMarkerPoints);
	        }


            playerRb.AddForce(transform.forward * speed);
            
	    }
	  
    }

    public override void AgentReset()
    {
        Debug.LogError("Agent reset called!");

        player.transform.position = startPosition.position;
    }

    public override void AgentOnDone()
    {

    }
}
