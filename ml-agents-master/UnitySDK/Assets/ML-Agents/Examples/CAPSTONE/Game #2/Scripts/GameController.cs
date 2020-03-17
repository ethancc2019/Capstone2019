using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private GameObject GameTwoGameObject;
    private GameObject player;
    private Game2Agent game2Agent;

	public Text timerText;

	public float timer = 60f;
	// Use this for initialization
	void Start ()
	{
		//Get the 
		timerText = GameObject.FindGameObjectWithTag("timer_text").GetComponent<Text>();
        game2Agent = player.GetComponent<Game2Agent>();
	}
	
	// Update is called once per frame
	void Update () {

        //Here we can stop training, rest the game environment for next training iteration, or debug game mechanics
	    if (game2Agent.score < 0)
	    {
	        Debug.Log("Game over");
	       // EditorApplication.ExecuteMenuItem("Edit/Play");

        }
		if(this.timer <= 0.0)
		{
			//EditorApplication.ExecuteMenuItem("Edit/Play");

		}
		this.timer -= Time.deltaTime;
		timerText.text = this.timer.ToString("#.0");
	}
}
