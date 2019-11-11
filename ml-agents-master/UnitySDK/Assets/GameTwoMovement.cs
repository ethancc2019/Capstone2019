using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTwoMovement : MonoBehaviour
{

    //Speeds
    public float rotationSpeed = 200.0f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Deal with rotation first
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
        transform.Rotate(0, 0, -rotation);

        //Translation is an special case, only occurs if the player wants to go up!
        if (Input.GetAxis("Vertical") > 0)
        {
            Debug.Log("Vertical input!");
            Vector2 translation = Input.GetAxis("Vertical") * new Vector2(0f, 10f);
            translation *= Time.deltaTime;
            GetComponent<Rigidbody2D>().AddRelativeForce(translation, ForceMode2D.Impulse);
        }
    }

}

