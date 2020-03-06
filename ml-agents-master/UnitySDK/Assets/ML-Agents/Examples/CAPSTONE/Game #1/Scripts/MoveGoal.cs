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
    public void Move()
    {
        float x = floor.transform.position.x + UnityEngine.Random.Range(-6.0f, 6.0f);
        float z = floor.transform.position.z + UnityEngine.Random.Range(-6.0f, 6.0f);
        goal.x = x;
        goal.z = z;
        this.transform.position = goal;
    }

    public void OnTriggerEnter(Collider col)
    { 
        if((col.transform.tag != "floor") && (col.transform.tag != "Player"))
        {
            Debug.Log("Collider transform tag: " + col.transform.tag);
            if (col.transform == GetComponent<Game1Agent>().GetCurrentGoal().transform)
            {
                Debug.Log("Moving current goal.");
                Move();
            }
        }
    }
}
