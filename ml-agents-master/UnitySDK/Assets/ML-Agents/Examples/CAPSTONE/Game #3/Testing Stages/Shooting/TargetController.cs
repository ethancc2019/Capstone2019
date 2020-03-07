using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private static int numOfActiveTargets = 7;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (numOfActiveTargets == 0)
        {
            //reset
            Debug.Log("All targets hit!");
        }
    }

    

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Target Hit!");
        numOfActiveTargets--;
        //Add reward here
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
