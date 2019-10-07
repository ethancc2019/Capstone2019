﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using Random = UnityEngine.Random;

public class TemplateAgent : Agent
{

    public float speed = 3f;
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
        AddVectorObs(player.transform.position.y);
        AddVectorObs(player.transform.position.z);


    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

        //Here get the action and apply it to the player

        int counter = 0;
        Debug.Log("Discrete space type: " + counter);
        float action = vectorAction[0];
        //float index = vectorAction[random];
        player.GetComponent<Rigidbody>().AddForce(Vector3.forward);




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
