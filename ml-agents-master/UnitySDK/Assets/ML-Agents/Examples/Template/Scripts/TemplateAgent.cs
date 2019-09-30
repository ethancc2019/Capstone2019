using System;
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
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.discrete)
        {
            int counter = 0;
            Debug.Log("Discrete space type: " + counter);
            int random = Random.Range(0, 4);
            float index = vectorAction[random];
            player.GetComponent<Rigidbody>().AddForce(Vector3.forward);
            counter++;
            switch ((int)index)
            {
                case 0: //W
                    player.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed);
                    if (goal.transform.position.x - player.transform.position.x <= 0f)
                    {
                        Debug.Log("Reward earned!");
                        SetReward(OnMarkerPoints);
                        Done();
                    }

                    break;
                case 1: //A
                    player.GetComponent<Rigidbody>().AddForce(Vector3.left * speed);
                    if (goal.transform.position.x - player.transform.position.x <= 0f)
                    {
                        Debug.Log("Reward earned!");
                        SetReward(OnMarkerPoints);
                        Done();
                    }
                    break;
                case 2: //S
                    player.GetComponent<Rigidbody>().AddForce(Vector3.back * speed);
                    if (goal.transform.position.x - player.transform.position.x <= 0f)
                    {
                        Debug.Log("Reward earned!");
                        SetReward(OnMarkerPoints);
                        Done();
                    }
                    break;
                case 3: //W
                    player.GetComponent<Rigidbody>().AddForce(Vector3.right * speed);
                    if (goal.transform.position.x - player.transform.position.x <= 0f)
                    {
                        Debug.Log("Reward earned!");
                        SetReward(OnMarkerPoints);
                        Done();
                    }
                    break;
            }
            //player.transform.Translate(speed * SendKeys * Time.deltaTime, 0f, speed * Input.GetAxis("Vertical") * Time.deltaTime);
        }



        //       playerRb.AddForce(transform.forward * speed);

        //}

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
