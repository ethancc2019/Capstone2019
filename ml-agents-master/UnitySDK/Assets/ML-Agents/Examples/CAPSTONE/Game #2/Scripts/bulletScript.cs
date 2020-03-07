using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Input;

public class bulletScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "asteroid")
        {
            Destroy(collider.gameObject);
        }

        else if(collider.tag == "Finish")
        {
            Destroy(gameObject);
            Debug.Log("Destroy Bullet");
        }
    }
}
