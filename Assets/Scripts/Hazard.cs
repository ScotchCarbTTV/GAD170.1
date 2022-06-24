using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    //variable for reference to the PlayerController
    [SerializeField] PlayerControllerThatLevelsUp pController;

    //variable for referencing the player health component on the player
    private PlayerHealth pHealth;
    //variable for checking if the player is currently invulnerable
    private bool invuln;

    private void Start()
    {
        //assigning the player health and player controller variables
        GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerHealth>(out pHealth);
        GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerControllerThatLevelsUp>(out pController);
        //set invuln to false 
        invuln = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if an object entering the collider is the Player
        if(other.tag == "Player")
        {
            //check if they are currently invulnerable
            if (invuln == false)
            {
                if(pHealth.currentHealth != 0)
                {
                    //call the Invuln method
                    StartCoroutine(Invuln()); 
                }
            }
        }
    }

    //Enumerator for handling when the player is detected in the trigger
    IEnumerator Invuln()
    {
        //check that losing health won't put the player on 0 health
        if (pHealth.currentHealth > 1)
        {
            //set invuln to true
            invuln = true;
            //deduct health from the player
            pHealth.LoseHealth();
            //wait for a moment
            yield return new WaitForSeconds(1.5f);
            //remove invulnerability
            invuln = false;
        }
        else
        {
            //remove the last remaining point of health from the player
            pHealth.LoseHealth();
            //set invuln to true (to stop the death method for activating repeatedly)
            invuln = true;
            //call the Death method on the player controller (specifically the ground based death to losing health from a hazard)
            pController.Death(3);
        }
    }
}
