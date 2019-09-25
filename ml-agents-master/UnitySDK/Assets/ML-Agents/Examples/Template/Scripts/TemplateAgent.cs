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
            Debug.Log("Discrete space type!");
            int random = Random.Range(0, 4);
            float index = vectorAction[random];
            switch ((int)index)
            {
                case 0: //W
                    player.GetComponent<Rigidbody>().AddForce(Vector3.forward);
                    break;
                case 1: //A
                    player.GetComponent<Rigidbody>().AddForce(Vector3.left);
                    break;
                case 2: //S
                    player.GetComponent<Rigidbody>().AddForce(Vector3.back);
                    break;
                case 3: //W
                    player.GetComponent<Rigidbody>().AddForce(Vector3.right);
                    break;
            }
            //player.transform.Translate(speed * SendKeys * Time.deltaTime, 0f, speed * Input.GetAxis("Vertical") * Time.deltaTime);
        }

        if (Mathf.Abs(goal.transform.position.x - player.transform.position.x) <= DistanceToEarnReward)
        {
            Debug.Log("Reward earned!");
            SetReward(OnMarkerPoints);
            Done();
        }


        //       playerRb.AddForce(transform.forward * speed);

        //}

    }

    //void OnCollisionEnter(Collision col)
    //{
    //    // Touched goal.
    //    if (col.gameObject.CompareTag("goal"))
    //    {
    //        AddReward(1f);
    //        AgentReset();
    //    }
    //}

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
            AgentReset();
        }
    }
    public override void AgentOnDone()
    {

    }
}
