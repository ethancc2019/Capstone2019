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
    private static List<Vector3> initialWallPositions = new List<Vector3>(); //positions relative to parent
    private static List<Vector3> initialPlatformPositions = new List<Vector3>();
    private static List<Vector3> initialGoalPositions = new List<Vector3>();
    private List<GameObject> wallsList = new List<GameObject>();
    private List<GameObject> goalsList = new List<GameObject>();
    private List<GameObject> platformsList = new List<GameObject>();
    public void Start()
    {
        findObjects();
    }
    private void AddDescendantsWithTag(Transform parent, string tag, List<GameObject> list, List<Vector3> vectorList)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == tag)
            {
                list.Add(child.gameObject);
                vectorList.Add(child.gameObject.transform.localPosition);
            }
            AddDescendantsWithTag(child, tag, list, vectorList);
        }
    }
    public void findObjects()
    {
        //finding-all-children-of-object recursion
        //Finds objects only relative to the Area's hierarchy(hopefully)
        AddDescendantsWithTag(this.transform, "wall", wallsList, initialWallPositions);
        AddDescendantsWithTag(this.transform, "goal", goalsList, initialGoalPositions);
        AddDescendantsWithTag(this.transform, "platform", platformsList, initialPlatformPositions);
    }
    public void RandomizeWalls()
    {
        //Moves walls around randomly to help with training.
        int index = 0;
        foreach (GameObject wall in wallsList)
        {
            float randomX = Random.Range(-2.5f, 2.5f);
            float randomY = Random.Range(-0.2f, 0.2f);
            float randomZ = Random.Range(-2.5f, 2.5f);
            Vector3 randomizedVector = new Vector3(randomX, randomY, randomZ);
            wall.transform.localPosition = initialWallPositions[index] + randomizedVector;
            index++;
        }
    }
    public void RandomizeGoals()
    {
        //Moves goals around randomly to help with training.
        int index = 0;
        foreach (GameObject goal in goalsList)
        {
            float randomX = Random.Range(-1.5f, 1.5f);
            float randomY = Random.Range(0f, 0.2f);
            float randomZ = Random.Range(-1.5f, 1.5f);
            Vector3 randomizedVector = new Vector3(randomX, randomY, randomZ);
            goal.transform.localPosition = initialGoalPositions[index] + randomizedVector;
            index++;
        }
    }
    public void RandomizePlatforms()
    {
        //Moves platforms around randomly to help with training.
        int index = 0;
        foreach (GameObject platform in platformsList)
        {
            float randomX = Random.Range(-2.5f, 2.5f);
            //float randomY = Random.Range(-0.2f, 0.2f);
            float randomZ = Random.Range(-2.5f, 0.5f);
            Vector3 randomizedVector = new Vector3(randomX, 0, 0);
            platform.transform.localPosition = initialPlatformPositions[index] + randomizedVector;
            index++;
        }
    }
    public GameObject getSpawnPointOne()
    {
        return spawnPointOne;
    }
    public override void ResetArea()
    {
        RandomizeWalls();
        RandomizeGoals();
        RandomizePlatforms();
    }
}
