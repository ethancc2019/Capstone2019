using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Text scoreText;
    public GameObject player;
    public static int score = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    scoreText.text = score.ToString();
	}

    public void increaseScore()
    {
        score++;
    }

    public void decreaseScore()
    {
        score--;
    }

}
