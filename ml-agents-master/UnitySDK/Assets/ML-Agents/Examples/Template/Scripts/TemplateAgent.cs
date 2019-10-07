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

        int counter = 0;
        var actionX = vectorAction[0];
        //var actionZ = vectorAction[1];
        Debug.Log("Action X: " + actionX.ToString());
        //Debug.Log("Action Z: " + actionZ.ToString());

        float action = vectorAction[0];
        transform.Translate(Vector3.forward);

        

    }

    void OnCollisionEnter(Collision col)
    {
        // Touched goal.
        if (col.gameObject.CompareTag("goal"))
        {
            AddReward(1f);
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
