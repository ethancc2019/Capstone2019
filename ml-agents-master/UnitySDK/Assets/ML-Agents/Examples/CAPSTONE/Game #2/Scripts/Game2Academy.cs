using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class Game2Academy : Academy
{
    public override void AcademyReset()
    {
        Asteroid.speed = resetParameters["asteroid_speed"];
        Asteroid.size = resetParameters["asteroid_size"];
        PowerUpSpawnner.size = resetParameters["goal_size"];
    }
}
