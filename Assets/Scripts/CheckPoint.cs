using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    //variable to create a reference to player controller script
    [SerializeField] PlayerControllerThatLevelsUp pController;

    //bool to make the checkpoints one time use only
    private bool _checked;

    private void Awake()
    {
        //set the checkpoint to unvisited
        _checked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks if an object entering the trigger is the player and sets their spawn to their current location & marks the checkpoint as visited.
        if (other.TryGetComponent<PlayerControllerThatLevelsUp>(out pController))
        {
            if (!_checked)
            {
                pController.SetRespawn();
                _checked = true;
                //toggle the checkpoint object animation to true
            }
        }
    }
}
