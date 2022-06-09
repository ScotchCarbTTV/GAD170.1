using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private PlayerControllerThatLevelsUp pController;

    private void Start()
    {
        GameObject gobject;
        gobject = GameObject.FindGameObjectWithTag("Player");
        gobject.TryGetComponent<PlayerControllerThatLevelsUp>(out pController);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            pController.Respawn();
        }
    }
}
