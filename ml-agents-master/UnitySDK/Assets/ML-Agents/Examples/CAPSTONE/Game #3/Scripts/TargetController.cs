using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public bool isDestroyed;
    private void Start()
    {
        isDestroyed = false;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("bullet") && isDestroyed == false)
        {
            isDestroyed = true;
            Debug.Log("Target Hit!");
            //numOfActiveTargets--;
            //Add reward here
            GetComponent<SpriteRenderer>().color = Color.red;
            Destroy(gameObject, 1f);
            GameObject temp = this.transform.parent.Find("Target Container").gameObject;
            temp.GetComponent<TargetSpawnner>().activeTargets--;
            //GameObject.Find(this.transform.parent.ToString() + "/target_container").GetComponent<TargetSpawnner>().activeTargets--;
        }
        
    }
}
