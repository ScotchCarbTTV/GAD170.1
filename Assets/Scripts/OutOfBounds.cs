using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private PlayerControllerThatLevelsUp pController;
    private PlayerHealth pHealth;
    private bool invuln;

    private void Start()
    {
        invuln = false;
        GameObject gobject;
        gobject = GameObject.FindGameObjectWithTag("Player");
        gobject.TryGetComponent<PlayerControllerThatLevelsUp>(out pController);
        gobject.TryGetComponent<PlayerHealth>(out pHealth);
    }

    private void OnTriggerExit(Collider other)
    {
        
        if(other.tag == "Player")
        {
            if (pHealth.currentHealth > 0)
            {
                if (invuln == false)
                {
                    StartCoroutine(Invuln());
                }
            }
            else
            {
                pController.Death(2);
            }
        }
    }

    IEnumerator Invuln()
    {
        invuln = true;
        pHealth.LoseHealth();
        pController.Death(1);
        yield return new WaitForSeconds(4f);
        invuln = false;
    }
}
