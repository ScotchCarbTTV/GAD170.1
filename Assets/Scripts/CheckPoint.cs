using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    //refrence to player controller script
    [SerializeField] PlayerControllerThatLevelsUp pController;

    private bool _checked;

    private void Awake()
    {
        _checked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerControllerThatLevelsUp>(out pController))
        {
            if (!_checked)
            {
                pController.SetRespawn();
                _checked = true;
            }
        }
    }
}
