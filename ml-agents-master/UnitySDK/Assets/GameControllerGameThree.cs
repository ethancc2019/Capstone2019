using UnityEngine;

public class GameControllerGameThree : MonoBehaviour
{

    public bool resetArea = false;
    public bool gameOver = false;

    //Refrence to the Power Ups
    public GameObject powerUpContainer;
    private PowerUpSpawnGameThree powerUpScript;

    public GameObject targetContainer;
    private TargetSpawnner targetScript;

    public GameObject playerContainer;
    private Shooting playerAmmoDetails;
    // Start is called before the first frame update
    void Start()
    {
        powerUpScript = powerUpContainer.GetComponent<PowerUpSpawnGameThree>();
        targetScript = targetContainer.GetComponent<TargetSpawnner>();
        playerAmmoDetails = playerContainer.GetComponent<Shooting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAmmoDetails.ammoCount == 0 && targetScript.activeTargets > 1 && powerUpScript.activePowerUps <= 0)
        {
            //Bad player still has active targets and cannot pick up any more ammo.
            //Give player bad reward or call agent reset, set resetArea to true
            Debug.Log("Player Ran out of ammo and powerups");
            gameOver = true;
            resetArea = true;
        }

        if (targetScript.activeTargets == 0)
        {
            //Give player reward
            // Rest area for new iteration
            gameOver = true;
            resetArea = true;
            Debug.Log("All Targets Hit!");
        }
    }

    public void ResetGameArea()
    {
        RestPowerups();
        ResetTargets();
        ResetPlayer();
        resetArea = false;
    }

    private void ResetPlayer()
    {
        throw new System.NotImplementedException();
    }

    private void ResetTargets()
    {
        throw new System.NotImplementedException();
    }

    private void RestPowerups()
    {
        throw new System.NotImplementedException();
    }
}
