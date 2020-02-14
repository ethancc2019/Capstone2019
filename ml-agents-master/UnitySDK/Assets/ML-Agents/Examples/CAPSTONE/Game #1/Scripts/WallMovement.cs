using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour {
    public int initialDirection; //1 for up/down, 2 for left/right
    public Vector3 difference;
    public Vector3 startPosition;
    private Vector3 endPosition;
    private float min;
    private float max;
    private float timer;
    public float seconds;
    private bool moveDirection;
    private float percent;
	// Use this for initialization
	void Start () {
        startPosition = this.transform.position;
        endPosition = this.transform.position - difference;
        min = this.transform.position.y;
        max = endPosition.y;

        moveDirection = true;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(endPosition);
        Debug.Log("min:" + min);
        Debug.Log(max);
        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * 2, max - min) + min, transform.position.z);
    }
}
