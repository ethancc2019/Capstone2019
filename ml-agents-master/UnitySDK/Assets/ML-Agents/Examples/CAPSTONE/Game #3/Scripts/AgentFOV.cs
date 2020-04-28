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

    public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> visiblePowerups = new List<Transform>();
    void Start()
    {
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
        visibleTargets.Clear();
        visiblePowerups.Clear();
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
                    visibleTargets.Add(target);
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
                    visiblePowerups.Add(powerup);
                }
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
