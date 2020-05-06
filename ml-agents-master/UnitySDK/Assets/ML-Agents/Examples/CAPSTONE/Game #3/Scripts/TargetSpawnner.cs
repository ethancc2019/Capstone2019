using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetSpawnner : MonoBehaviour
{

    public GameObject[] targetSpawnPoints;
    public GameObject TargetPrefabGameObject;
    public int maxNumOfTargetsToSpawn;
    public int activeTargets;

    public List<GameObject> ActiveTargetsGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        activeTargets = maxNumOfTargetsToSpawn;
        //spawnTargets();
    }


    public void spawnTargets()
    {
        ActiveTargetsGameObjects.Clear();
        List<int> tempPoint = new List<int>();
        int randomIndex = 0;
        for (int i = 0; i < maxNumOfTargetsToSpawn; i++)
        {
            RESTART:
            randomIndex = Random.Range(0, targetSpawnPoints.Length); //Get random index from the array
            if (!tempPoint.Contains(randomIndex))
            {
                tempPoint.Add(randomIndex);
                GameObject temp = Instantiate(TargetPrefabGameObject, targetSpawnPoints[randomIndex].transform.position, Quaternion.identity);
                temp.transform.parent = this.transform.parent;
                //hardcoded value, needs to be updated according to layer value in unity editor!
                temp.layer = 15; 
                ActiveTargetsGameObjects.Add(temp);
            }
            else
            {
                goto RESTART; //Random index used choose a new one
            }
            

        }
        //activeTargets = maxNumOfTargetsToSpawn;

    }

    public void destroyAllTargets()
    {
        if (ActiveTargetsGameObjects.Count > 0)
        {
            foreach (var activeTarget in ActiveTargetsGameObjects.ToList())
            {
                Destroy(activeTarget);
                ActiveTargetsGameObjects.Remove(activeTarget);
            }
        }
        
    }
}
