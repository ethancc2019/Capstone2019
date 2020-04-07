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

    private int playerAmmoCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        powerUpScript = powerUpContainer.GetComponent<PowerUpSpawnGameThree>();
        targetScript = targetContainer.GetComponent<TargetSpawnner>();
        

        targetScript.spawnTargets();
        powerUpScript.spawnPowerUps();
        playerAmmoCount = player.GetComponent<Shooting>().ammoCount;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerAmmoCount == 0 && targetScript.activeTargets >= 1 && powerUpScript.activePowerUps <= 0)
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
        //Debug.Log("Active Targets: " + targetScript.activeTargets);

        


    }

   

    public void ResetGameArea()
    {

        Debug.Log("Ammo Count: " + playerAmmoCount);
        Debug.Log("PowerUp Count: " + powerUpScript.activePowerUps.ToString());
        Debug.Log("Target Count: " + targetScript.activeTargets.ToString());

        RestPowerups();
        //ResetTargets();
       // ResetPlayer();
        gameOver = false;

    }

    public void ResetPlayer()
    {
        //Here choose new spawn point for the player
        playerSpawnContainer.GetComponent<SpawnPointController>().SpawnPlayerInRandomPoint();
        player.GetComponent<Shooting>().ammoCount = 5;
    }

    private void ResetTargets()
    {
        targetContainer.GetComponent<TargetSpawnner>().destroyAllTargets();
        targetContainer.GetComponent<TargetSpawnner>().spawnTargets();
        targetScript.activeTargets = targetContainer.GetComponent<TargetSpawnner>().activeTargets;
        //throw new System.NotImplementedException();
    }

    private void RestPowerups()
    {
        //Here, delete all powerup and calculate new spawn locations
        powerUpContainer.GetComponent<PowerUpSpawnGameThree>().DestroyAllPowerups();
        powerUpContainer.GetComponent<PowerUpSpawnGameThree>().spawnPowerUps();
        powerUpScript.activePowerUps = powerUpContainer.GetComponent<PowerUpSpawnGameThree>().numOfPowerUpsToSpawn;
    }
}
