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

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    while (visibleTargets[targetCounter%10] != null)
                    {
                        targetCounter++;
                    }
                    visibleTargets[targetCounter%10] = target.gameObject;
                    targetCounter++;
                }
            }
        }
        for (int i = 0; i < powerupsInViewRadius.Length; i++)
        {
            Transform powerup = powerupsInViewRadius[i].transform;
            Vector2 dirToPowerup = (powerup.position - transform.position).normalized;
            if (Vector2.Angle(transform.up, dirToPowerup) < viewAngle / 2)
            {
                float dstToPowerup = Vector2.Distance(transform.position, powerup.position);

                if (!Physics2D.Raycast(transform.position, dirToPowerup, dstToPowerup, obstacleMask))
                {
                    while(visiblePowerups[powerupCounter%10] != null)
                    {
                        powerupCounter++;
                    }
                    visiblePowerups[powerupCounter%10] = powerup.gameObject;
                    powerupCounter++;
                }
            }
        }
        for(int i = 0; i < visiblePowerups.Length; i++)
        {
            //if(visiblePowerups[i] != null)
            //{
            //    powerupTransforms[i] = visiblePowerups[i].transform;
            //}
            if (visibleTargets[i] != null)
            {
                targetTransforms[i] = visibleTargets[i].transform.position;
            }
        }
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
