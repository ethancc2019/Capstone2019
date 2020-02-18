using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class Game1Academy : Academy
{
    private Game1Area[] game1Areas;
    // Start is called before the first frame update
    public override void AcademyReset()
    {
        /*
        if(game1Areas == null)
        {
            game1Areas = FindObjectsOfType<Game1Area>();
        }

        foreach(Game1Area game1Area in game1Areas)
        {
            game1Area.ResetArea();
        }
        */
    }
}
