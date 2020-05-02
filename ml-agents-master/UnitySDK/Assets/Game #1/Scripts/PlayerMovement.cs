using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Variables
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float fallMultiplier = 2.5f;
    private Vector3 moveDirection = Vector3.zero;
    private GameObject spawnPointOne;

    public Game1Area area;

    //Scripts 
    //public GameObject gameManger;

    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        // is the controller on the ground?
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        else
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), moveDirection.y, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= speed;
            moveDirection.z *= speed;
        }
        //Applying gravity to the controller
        moveDirection.y -= (-Physics2D.gravity.y) * (fallMultiplier - 1) * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        //Debug.Log("moveDirection: " + moveDirection + "vel: " + controller.velocity);

        //Player fell off platform, reset him to the first spawn point
        if (this.transform.position.y <= this.area.transform.position.y)
        {
            ResetPlayer();
        }

    }

    public void ResetPlayer()
    {
        //gameManger.GetComponent<GameManager>().decreaseScore();
        spawnPointOne = this.area.getSpawnPointOne();
        this.transform.position = spawnPointOne.transform.position;
    }
}