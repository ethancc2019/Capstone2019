using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
  
   
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Target Hit!");
        //numOfActiveTargets--;
        //Add reward here
        GetComponent<SpriteRenderer>().color = Color.red;
        Destroy(gameObject,1f);
        GameObject.FindGameObjectWithTag("target_container").GetComponent<TargetSpawnner>().activeTargets--;
    }
}
