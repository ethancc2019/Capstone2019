using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private GameObject playerGameObject;

    private GameObject PowerUpContainer;
    // Start is called before the first frame update
    void Start()
    {
        //playerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("+5 Ammo Added!");
            playerGameObject = this.transform.parent.Find("Player_1").gameObject;
            playerGameObject.GetComponent<Shooting>().ammoCount += 5;
            //GameObject.FindGameObjectWithTag("power_container").GetComponent<PowerUpSpawnGameThree>().activePowerUps--;
            GameObject tempPowerUp = this.gameObject;
            GameObject tempContainer = this.transform.parent.Find("Power Up Container").gameObject;
            tempContainer.GetComponent<PowerUpSpawnGameThree>().activePowerUps--;
            //GameObject temp = gameObject;
            Destroy(tempPowerUp);
        }
            
        
    }
}
