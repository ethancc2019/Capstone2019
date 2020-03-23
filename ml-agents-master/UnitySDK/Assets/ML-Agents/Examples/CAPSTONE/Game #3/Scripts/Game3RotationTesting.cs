using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game3RotationTesting : MonoBehaviour
{
    private static int lifeIndex = 2;
    public GameObject[] lives;

    public float speed = 5f;
    private Rigidbody2D rb;

    private Vector2 movement;
    private Vector2 mousePosition;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (lifeIndex < 0) //Player has 3 lives. When he is hit by a bullet decrement by 1. When lifeIndex is 0 game is over reset environment
        {
            //One of the players lost all three lives
            Debug.Log("Game Over!");
        }

        //Mouse rotations
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        //Bind player in screen bounds
        Vector3 pos = cam.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = cam.ViewportToWorldPoint(pos);

    }

    void FixedUpdate()
    {
        

        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");
        //rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("bullet"))
        {
            Debug.Log("Hit by bullet!: " + lifeIndex.ToString());
            GameObject tempLife = lives[lifeIndex];
            Destroy(tempLife);
            lifeIndex--;
        }
    }
}
