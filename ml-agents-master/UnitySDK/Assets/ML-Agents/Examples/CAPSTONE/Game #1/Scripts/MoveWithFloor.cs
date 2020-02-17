using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithFloor : MonoBehaviour {
	private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "player")
        {
            col.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "player")
        {
            col.gameObject.transform.parent = null;
        }
    }
}
