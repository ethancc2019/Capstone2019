using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//referenced from https://github.com/SebLague/Field-of-View/blob/master/Episode%2001/Scripts/FieldOfView.cs
//modified to work with Vector2 and 2D game
public class AgentFOV : MonoBehaviour
{

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask powerupMask;
    public LayerMask obstacleMask;
    int targetCounter = 0;
    int powerupCounter = 0;
    private GameObject[] visibleTargets = new GameObject[10];
    private GameObject[] visiblePowerups = new GameObject[10];
    public Vector3[] targetTransforms = new Vector3[10];
    public Vector3[] powerupTransforms = new Vector3[10];
    void Start()
    {
        targetTransforms = new Vector3[10];
        powerupTransforms = new Vector3[10];
        StartCoroutine("FindTargetsWithDelay", .2f);
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        //finds targets AND powerups
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), viewRadius, targetMask);
        Collider2D[] powerupsInViewRadius = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), viewRadius, powerupMask);
        bool[] targetIndicesFound;
        bool[] powerupIndicesFound;
        targetIndicesFound = AddNewTargets(targetsInViewRadius, visibleTargets);
        powerupIndicesFound = AddNewPowerups(powerupsInViewRadius, visiblePowerups);
        //Sets any old targets null
        for (int i = 0; i < targetIndicesFound.Length; i++)
        {
            if (!targetIndicesFound[i])
            {
                visibleTargets[i] = null;
            }
        }
        //sets any old powerups null
        for (int i = 0; i < powerupIndicesFound.Length; i++)
        {
            if (!powerupIndicesFound[i])
            {
                visiblePowerups[i] = null;
            }
        }
        //sets the transform arrays' elements
        for (int i = 0; i < visiblePowerups.Length; i++)
        {
            if(visiblePowerups[i] != null && visiblePowerups[i].transform.position  != Vector3.zero)
            {
                powerupTransforms[i] = visiblePowerups[i].transform.position;
            }
            else
            {
                powerupTransforms[i] = Vector3.zero;
            }
            if (visibleTargets[i] != null && visibleTargets[i].transform.position != Vector3.zero)
            {
                targetTransforms[i] = visibleTargets[i].transform.position;
            }
            else
            {
                targetTransforms[i] = Vector3.zero;
            }
        }
    }

    bool[] AddNewTargets(Collider2D[] from, GameObject[] to)
    {
        //Returns a bool array of which transforms to return from the gameObj array.
        bool[] targetIndicesFound = new bool[10];
        for (int i = 0; i < from.Length; i++)
        {
            Transform target = from[i].transform;
            if (target.gameObject != null)
            {
                bool targetFound = false;
                Vector2 dirToTarget = (target.position - transform.position).normalized;
                if (Vector2.Angle(transform.up, dirToTarget) < viewAngle / 2)
                {
                    for (int j = 0; j < to.Length && targetFound == false; j++)
                    {
                        if (target.gameObject == to[j])
                        {
                            targetFound = true;
                            targetIndicesFound[j] = true;
                        }
                    }
                    if (!targetFound)
                    {
                        float dstToTarget = Vector2.Distance(transform.position, target.position);

                        if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                        {
                            while (to[targetCounter % 10] != null)
                            {
                                targetCounter++;
                            }
                            to[targetCounter % 10] = target.gameObject;
                            targetIndicesFound[targetCounter % 10] = true;
                            targetCounter++;
                        }
                    }
                }
            }
        }
        return targetIndicesFound;
    }
    bool [] AddNewPowerups(Collider2D[] from, GameObject[] to)
    {
        //Returns a bool array of which transforms to return from the gameObj array.
        bool[] powerupIndicesFound = new bool[10];
        for (int i = 0; i < from.Length; i++)
        {
            Transform powerup = from[i].transform;     
            if (powerup.gameObject != null)
            {
                bool powerupFound = false;
                Vector2 dirToPowerup = (powerup.position - transform.position).normalized;
                if (Vector2.Angle(transform.up, dirToPowerup) < viewAngle / 2)
                {
                    //search for pre-existing element in array, want no duplicates.
                    for (int j = 0; j < to.Length && powerupFound == false; j++)
                    {
                        if (powerup.gameObject == to[j])
                        {
                            powerupFound = true;
                            powerupIndicesFound[j] = true;
                        }
                    }
                    if (!powerupFound)
                    {
                        float dstToPowerup = Vector2.Distance(transform.position, powerup.position);
                        if (!Physics2D.Raycast(transform.position, dirToPowerup, dstToPowerup, obstacleMask))
                        {
                            //add to array
                            while (to[powerupCounter % 10] != null)
                            {
                                powerupCounter++;
                            }
                            to[powerupCounter % 10] = powerup.gameObject;
                            powerupIndicesFound[powerupCounter % 10] = true;
                            powerupCounter++;
                        }
                    }
                }
            }
        }
        return powerupIndicesFound;
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z+90f;
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
