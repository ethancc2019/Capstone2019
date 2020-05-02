using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class Game3Agent : Agent
{
    private Vector2 movement;
    private Rigidbody2D rb;
    public float speed = 5f;
    public static float turnSpeed = 5f;

    public Camera cam;
    public RayPerception2D rayPerception;
    public Transform gameArea;


    public Shooting shooting;

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (vectorAction[3] == 1f)
        {
            GetComponent<Shooting>().Shoot();
        }
        float angle = transform.rotation.z;
        //WASD Movement
        if (vectorAction[0] == 1f)
        {
            //W
            //movement.y = 1f;
        }
        else if (vectorAction[0] == 2f)
        {
            //S
            //movement.y = -1f;
        }
        else
        {
            //movement.y = 0f;
        }
        if (vectorAction[1] == 1f)
        {
            //A
            //movement.x = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            //D
            //movement.x = 1f;
        }
        else
        {
            //movement.x = 0;
        }
        if (vectorAction[2] == 1f)
        {
            //Rotate CCW
            rb.rotation += 1 * turnSpeed;
        }
        else if (vectorAction[2] == 2f)
        {
            //Rotate CW
            rb.rotation += -1 * turnSpeed;
        }
        Mathf.Clamp(rb.rotation, 0, 360);
        rb.AddForce(movement * speed / 30 + -(rb.velocity * Time.deltaTime / 50));
        Vector3 pos = cam.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = cam.ViewportToWorldPoint(pos);
        AddReward(-1f / agentParameters.maxStep);
    }
    public override void InitializeAgent()
    {
        transform.position = transform.parent.transform.position;
        rb = GetComponent<Rigidbody2D>();
        rayPerception = GetComponent<RayPerception2D>();
        gameArea = transform.parent;
        shooting = GetComponent<Shooting>();
    }

    public override void CollectObservations()
    {
        //(transform.pos.x - gameArea.pos.x) / sizeOfGameAreaX;
        //(transform.pos.y - gameArea.pos.y) / sizeOfGameAreaY;
        //(transform.eulerAngles.z / 360f);
        //(rb.velocity) / maxVelocity;
        AddVectorObs((transform.position.x - gameArea.position.x)/ 8.55f);
        AddVectorObs((transform.position.y - gameArea.position.y)/ 4.81f);
        AddVectorObs(transform.eulerAngles.z / 360f);

        //powerupsCount / maxPowerUps;
        //targetCount / maxTargets;
        //ammoCount / (currentAmmo + powerupsCount * 3);
        AddVectorObs(shooting.ammoCount/ ())

        //foreach targetsInFOV[i]
        //{
        //    (targetsInFOV[i].pos.x - gameArea.pos.x) / sizeOfGameAreaX;
        //    (targetsInFOV[i].pos.y - gameArea.pos.y) / sizeOfGameAreaY;
        //    maybe also x & y velocity of targets if we can moving target training
        //}

        //foreach powerupsInFOV[i]
        //{
        //    (powerupsInFOV[i].pos.x - gameArea.pos.x) / sizeOfGameAreaX;
        //    (powerupsInFOV[i].pos.y - gameArea.pos.y) / sizeOfGameAreaY;
        //}

        //rayCasts at -45, 0, and 45 degree positions, relative to front of player, for wall detection
        float distance = 20f;
        float[] angles = { 0, 45, 315 };
        string[] detectableObjects = { "wall", "target", "powerup" };
        AddVectorObs(rayPerception.Perceive(distance, angles, detectableObjects));
    }
}
