using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using Random = UnityEngine.Random;

public class TemplateAgent : Agent
{

    public float speed = 1.0f;
    public float DistanceToEarnReward = 0.1f;
    public float OnMarkerPoints = 1f;

    public GameObject player;
    public Transform startPosition;
    public Rigidbody playerRb;
    public GameObject goal;
    Rigidbody rBody;


    public override void CollectObservations()
    {
        AddVectorObs(player.transform.position - goal.transform.position); // distance to target
        AddVectorObs(player.transform.position.x);
        AddVectorObs(player.transform.position.z);


    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

        //Here get the action and apply it to the player
        Debug.Log("Agent Action Called");

        int actionX = Mathf.FloorToInt(vectorAction[0]);
        var actionZ = vectorAction[1];
        Debug.Log("Length of vectoAction array: " + vectorAction.Length.ToString());
        
        Vector3 dirToGo = new Vector3();
        switch (actionX)
        {
            case 1:
                dirToGo = transform.forward;
                transform.Translate(dirToGo);
                Debug.Log("Action: " + actionX);
                break;
            case 2:
                dirToGo = -transform.forward;
                transform.Translate(dirToGo);
                Debug.Log("Action: " + actionX);
                break;
            case 3:
                dirToGo = Vector3.left; //LEFT
                transform.Translate(dirToGo);
                Debug.Log("Action: " + actionX);

                break;
            case 4:
                dirToGo = Vector3.right; //right
                transform.Translate(dirToGo);
                Debug.Log("Action: " + actionX);


                break;
        }

        float action = vectorAction[0];
        
        float distanceToTarget = Vector3.Distance(this.player.transform.position, goal.transform.position);

        if(distanceToTarget < 1.0f)
        {
            Debug.Log("Goal reached!");

            SetReward(1f);
            AgentReset();
        }

        if (this.player.transform.position.y < -1)
        {
            Debug.Log("Player fell!");
            SetReward(0.1f);

            AgentReset();
        }


    }

    void OnCollisionEnter(Collision col)
    {
        // Touched goal.
        if (col.gameObject.CompareTag("goal"))
        {
            AddReward(1f);
            AgentReset();
        }

        if (col.gameObject.CompareTag("block"))
        {
            Debug.Log("Player hit wall");
            AddReward(-1f);
            AgentReset();
        }
    }

    public override void AgentReset()
    {
        this.player.transform.position = startPosition.position;

    }

    void Update()
    {

        if (this.transform.position.y < -2f)
        {
            // If the Agent fell, zero its momentum
            Debug.Log("Player fell!");
            SetReward(0.1f);
            AgentReset();
        }

        //if (player.gameObject.CompareTag("goal"))
        //{
        //    Debug.Log("Reward earned!");
        //    SetReward(OnMarkerPoints);
        //    Done();
        //}

        //if (goal.transform.position - player.transform.position < DistanceToEarnReward)
        //{
        //    Debug.Log("Reward earned!");
        //    SetReward(OnMarkerPoints);
        //    Done();
        //}

    }
    public override void AgentOnDone()
    {

    }
}
