using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;
public class Game1Area : Area
{
    // Start is called before the first frame update
    public Game1Agent game1Agent;
    public TextMesh cumulativeRewardText;
    public GameObject spawnPointOne;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject getSpawnPointOne()
    {
        return spawnPointOne;
    }
}
