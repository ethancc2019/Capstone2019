using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class BallAgent : Agent
{
    public Rigidbody rigidbody;
    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public Transform Target;
    public override void AgentReset()
    {

    }

    public override void CollectObservations()
    {
        base.CollectObservations();
        AddVectorObs(this.transform.position);
        AddVectorObs(Target.position);
        AddVectorObs(rigidbody.velocity.x);
        AddVectorObs(rigidbody.velocity.y);
        AddVectorObs(rigidbody.velocity.z);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        base.AgentAction(vectorAction, textAction);
    }
}
