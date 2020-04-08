using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerGameThree : MonoBehaviour
{

    public bool gameOver = false;

    //Reference to the Power Ups
    public GameObject powerUpContainer;
    private PowerUpSpawnGameThree powerUpScript;

    //Reference to the Targets 
    public GameObject targetContainer;
    private TargetSpawnner targetScript;
    
    //Reference to player 
    public GameObject playerSpawnContainer;
    public GameObject player;
    private Shooting ammoDetails;

    private int playerAmmoCount = 0;



    // Start is called before the first frame update
    void Start()
    {
        powerUpScript = powerUpContainer.GetComponent<PowerUpSpawnGameThree>();
        targetScript = targetContainer.GetComponent<TargetSpawnner>();
        

        targetScript.spawnTargets();
        powerUpScript.spawnPowerUps();

        ammoDetails = player.GetComponent<Shooting>();

    }

    // Update is called once per frame
    void Update()
    {
        if (ammoDetails.ammoCount == 0 && targetScript.activeTargets > 0 && powerUpScript.activePowerUps <= 0)
        {
            //Bad player still has active targets and cannot pick up any more ammo.
            //Give player bad reward or call agent reset, set resetArea to true
            Debug.Log("Player Ran out of ammo and power ups");
            
            ResetGameArea();
        }

        if (targetScript.activeTargets == 0)
        {
            //Give player reward
            // Rest area for new iteration
            Debug.Log("All Targets Hit!");
            
            ResetGameArea();
        }
        Debug.Log("Ammo: " + ammoDetails.ammoCount);
        Debug.Log("Targets: " + targetScript.activeTargets);
        Debug.Log("Powerups: " + powerUpScript.activePowerUps);




    }



    public void ResetGameArea()
    {
        RestPowerups();
        ResetTargets();
        ResetPlayer();

    }

    public void ResetPlayer()
    {


        List<GameObject> powerUps = GetAllTagged(this.transform.parent, "bullet");

        foreach (var goal in powerUps)
        {
            Destroy(goal);
        }
        //Here choose new spawn point for the player
        playerSpawnContainer.GetComponent<SpawnPointController>().SpawnPlayerInRandomPoint();
        player.GetComponent<Shooting>().ammoCount = 5;
    }

    private void ResetTargets()
    {

        List<GameObject> activeTargets = GetAllTagged(this.transform.parent, "target");

        foreach (var target in activeTargets)
        {
            Destroy(target);
        }

        targetContainer.GetComponent<TargetSpawnner>().spawnTargets();
        targetScript.activeTargets = targetContainer.GetComponent<TargetSpawnner>().maxNumOfTargetsToSpawn;

       
    }

    private void RestPowerups()
    {

        List<GameObject> powerUps = GetAllTagged(this.transform.parent, "goal");

        foreach (var goal in powerUps)
        {
            Destroy(goal);
        }

        powerUpContainer.GetComponent<PowerUpSpawnGameThree>().spawnPowerUps();
        powerUpScript.activePowerUps = powerUpContainer.GetComponent<PowerUpSpawnGameThree>().numOfPowerUpsToSpawn;
    }


    public List<GameObject> GetAllTagged(Transform parent, string tag)
    {
        //searches down hierarchy for specific tagged GameObjs (Does not have to be in play area)
        List<GameObject> arrayOfTagged = new List<GameObject>();
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == tag)
            {
                arrayOfTagged.Add(child.gameObject);
            }
            GetAllTagged(child, tag);
        }
        return arrayOfTagged;
    }

}
