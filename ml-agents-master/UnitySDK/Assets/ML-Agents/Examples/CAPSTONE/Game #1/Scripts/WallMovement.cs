using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour {
    public int initialDirection; //1 for up/down, 2 for left/right
    public Vector3 lowerBound;
    public Vector3 upperBound;
    private Vector3 difference;
    private float timer;
    public float seconds;
    private bool moveDirection;
    private float percent;
	// Use this for initialization
	void Start () {
        upperBound = this.transform.position;
        difference = upperBound - lowerBound;
        moveDirection = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(timer <= seconds)
        {
            timer += Time.deltaTime;

            percent = timer / seconds;

            if(moveDirection = true)
            {
                this.transform.position = upperBound - difference * percent;
                if (this.transform.position == lowerBound)
                {
                    moveDirection = false;
                    timer = 0;
                }
                

            }
            else
            {
                this.transform.position = upperBound + difference * percent;
                if (this.transform.position == upperBound)
                {
                    moveDirection = true;
                    timer = 0;
                }
            }
        }
	}
}
