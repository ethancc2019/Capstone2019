using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGoal : MonoBehaviour
{
    public GameObject floor;
    private  Vector3 goal;
    void Start()
    {
        goal = this.transform.position;
    }
    public Vector3 Move()
    {
        float x = floor.transform.position.x + UnityEngine.Random.Range(-6.0f, 6.0f);
        float z = floor.transform.position.z + UnityEngine.Random.Range(-6.0f, 6.0f);
        goal.x = x;
        goal.z = z;
        return goal;
    }
}
