using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class TemplateAcademy : Academy
{

    public GameObject player;
    public Transform startPosition;
    public override void AcademyReset()
    {
        player.transform.position = startPosition.position;
        Debug.Log("Function Called!");
    }

    public override void AcademyStep()
    {
        Debug.Log("Agent Step called!");

    }

}
