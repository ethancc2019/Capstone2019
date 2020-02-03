using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private GameObject GameTwoGameObject;
    private GameTwoMovement gameTwoScript;
	// Use this for initialization
	void Start ()
	{
        //Get the 
	    gameTwoScript = GameObject.FindGameObjectWithTag("Player").GetComponent<GameTwoMovement>();
	}
	
	// Update is called once per frame
	void Update () {

        //Here we can stop training, rest the game environment for next training iteration, or debug game mechanics
	    if (gameTwoScript.score < 0)
	    {
	        Debug.Log("Game over");
	        EditorApplication.ExecuteMenuItem("Edit/Play");

        }
    }
}
