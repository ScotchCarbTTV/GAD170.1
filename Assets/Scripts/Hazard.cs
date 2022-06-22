using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    //reference to the PlayerController
    [SerializeField] PlayerControllerThatLevelsUp pController;

    private PlayerHealth pHealth;
    private bool invuln;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerHealth>(out pHealth);
        invuln = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (invuln == false)
            {
                StartCoroutine(Invuln());
            }
        }
    }

    IEnumerator Invuln()
    {
        if (pHealth.currentHealth > 0)
        {
            invuln = true;
            pHealth.LoseHealth();
            pController.Jump(2, 2);
            yield return new WaitForSeconds(1.5f);
            invuln = false;
        }
        else
        {
            invuln = true;
            pController.Death(3);
        }
    }
}
