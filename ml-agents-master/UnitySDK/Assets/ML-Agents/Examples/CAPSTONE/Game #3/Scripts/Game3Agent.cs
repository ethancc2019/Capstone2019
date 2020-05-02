using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class Game3Agent : Agent
{
    // Start is called before the first frame update
    private Vector2 movement;
    private Rigidbody2D rb;
    public float speed = 5f;
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
            movement.y = 1f;
        }
        else if (vectorAction[0] == 2f)
        {
            //S
            movement.y = -1f;
        }
        else
        {
            movement.y = 0f;
        }
        if (vectorAction[1] == 1f)
        {
            //A
            movement.x = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            //D
            movement.x = 1f;
        }
        else
        {
            movement.x = 0;
        }
        if (vectorAction[2] == 1f)
        {
            //Rotate CCW
           
        }
        else if (vectorAction[2] == 2f)
        {
            //Rotate CW

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
    }
}
