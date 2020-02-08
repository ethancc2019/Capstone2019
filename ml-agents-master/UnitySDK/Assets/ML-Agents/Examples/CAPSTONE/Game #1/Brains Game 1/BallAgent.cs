using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class BallAgent : Agent
{
    public Rigidbody rigidbody;
    private RayPerception3D rayPerception;
    public Transform Target;

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;


    public Transform spawnPointOne;
    public GameObject gameManger;
    private CharacterController controller;
    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rayPerception = GetComponent<RayPerception3D>();
        controller = GetComponent<CharacterController>();
        spawnPointOne = GameObject.FindGameObjectsWithTag("sp1")[0].transform;
        Target = GameObject.FindGameObjectsWithTag("target")[0].transform;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // is the controller on the ground?
        if (controller.isGrounded)
        {
            //Feed moveDirection with input.
            moveDirection = new Vector3(vectorAction[1], 0, vectorAction[0]);
            moveDirection = transform.TransformDirection(moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;
            //Jumping
            if (vectorAction[2] == 1)
                moveDirection.y = jumpSpeed;

        }
        //Applying gravity to the controller
        moveDirection.y -= gravity * Time.deltaTime;
        //Making the character move
        controller.Move(moveDirection * Time.deltaTime);
        AddReward(-1f / agentParameters.maxStep);
        //Player fell off platform, reset him to the first spawn point
        if (this.transform.position.y <= -1)
        {
            AddReward(-1f);
            AgentReset();
        }
    }
    public override void AgentReset()
    {
        this.transform.position = spawnPointOne.position;
    }

    public override void CollectObservations()
    {
        AddVectorObs(this.transform.position);
        AddVectorObs(Target.position);

        //Rayperception
        float rayDistance = 20f;
        float[] rayAngles = { 30f, 60f, 90f, 120f, 150f, 180f, 210f, 240f, 270f, 300f, 330f, 360f };
        string[] detectableObjects = { "killWall", "goal" };
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("killWall"))
        {
            AddReward(-1f);
            AgentReset();
        }
        else if (collision.transform.CompareTag("goal"))
        {
            AddReward(1f);
            AgentReset();
        }
    }

    
}
