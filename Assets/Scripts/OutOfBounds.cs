using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    //variable referencing the player controller script
    private PlayerControllerThatLevelsUp pController;
    //variable referencing the player health script
    private PlayerHealth pHealth;
    //bool for making the player invulnerable after being 'hit'
    private bool invuln;

    private void Start()
    {
        //set invuln to false
        invuln = false;
        //assign the player controller and player health variables
        GameObject gobject;
        gobject = GameObject.FindGameObjectWithTag("Player");
        gobject.TryGetComponent<PlayerControllerThatLevelsUp>(out pController);
        gobject.TryGetComponent<PlayerHealth>(out pHealth);
    }

    private void OnTriggerExit(Collider other)
    {
        
        if(other.tag == "Player")
        {
            //when the player leaves  the trigger (falls out of the world)
            //check if they have more than 0 health
            if (pHealth.currentHealth > 0)
            {
                //check if they're invulnerable (stops the method firing multiple times)
                if (invuln == false)
                {
                    //call the Invuln method
                    StartCoroutine(Invuln());
                }
            }
            else
            {
                //if they have no health remaining then call the Death method on the player controller (specifically the death by falling)
                pController.Death(2);
            }
        }
    }

    IEnumerator Invuln()
    {
        //set invuln to true
        invuln = true;
        //remove 1 health from the player
        pHealth.LoseHealth();
        //call the 'soft death' from the Death method on the player controller
        pController.Death(1);
        //wait for a few seconds
        yield return new WaitForSeconds(4f);
        //remove the player's invuln
        invuln = false;
    }
}
