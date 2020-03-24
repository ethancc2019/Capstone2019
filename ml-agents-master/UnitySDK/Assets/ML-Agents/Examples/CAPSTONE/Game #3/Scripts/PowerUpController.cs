using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private GameObject playerGameObject;

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
        if (collider.CompareTag("Player") || collider.CompareTag("Player_2"))
        {
            Debug.Log("+5 Ammo Added!");
            GameObject.FindGameObjectWithTag(collider.tag).GetComponent<Shooting>().ammoCount += 5; //Grab either player_1 or player_2
            GameObject.FindGameObjectWithTag("power_container").GetComponent<PowerUpSpawnGameThree>().activePowerUps--;
           
            //Debug.Log(GameObject.FindGameObjectWithTag("power_container").GetComponent<PowerUpSpawnGameThree>().activePowerUps.ToString());
            //Add +5 ammo to the player referencing his Shooting script
            GameObject temp = gameObject;
            Destroy(temp);
        }
    }
}
