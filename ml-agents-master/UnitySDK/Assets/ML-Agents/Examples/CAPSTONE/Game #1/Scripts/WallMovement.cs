using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour {
    private Vector3 startPoint;
    public Vector3 difference;
    private Vector3 endPoint;

    public float speed = 1.0f;

    private float startTime;

    private float length;
	// Use this for initialization
	void Start () {
        startPoint = this.transform.position;
        endPoint = startPoint - difference;

        startTime = Time.time;

        length = Vector3.Distance(startPoint, endPoint);
	}
	
	// Update is called once per frame
	void Update () {

        float distCovered = (Time.time - startTime) * speed;

        float percentDist = distCovered / length;

        this.transform.position = Vector3.Lerp(startPoint, endPoint, percentDist);

        if(this.transform.position == endPoint)
        {
            Swap();
        }
    }

    private void Swap()
    {
        Vector3 temp = startPoint;
        startPoint = endPoint;
        endPoint = temp;;
        startTime = Time.time;
    }
}
