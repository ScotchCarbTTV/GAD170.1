using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class Sign : MonoBehaviour
{
    //variable to reference the Cinemachine Freelook Camera
    [SerializeField] private CinemachineFreeLook freeLook;

    //variable to control UI panel
    [SerializeField] private GameObject panel;

    //variable for the camera focal on the player
    [SerializeField] private GameObject focal;
    [SerializeField] private GameObject signFocal;
    [SerializeField] private GameObject prompt;

    bool inRange;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("FreeLook").TryGetComponent<CinemachineFreeLook>(out freeLook);
        focal = GameObject.FindGameObjectWithTag("FreelookFocal");
        panel.SetActive(false);
        inRange = false;
        prompt.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && inRange == true)
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        if (panel.activeSelf == false)
        {
            panel.SetActive(true);
            freeLook.LookAt = signFocal.transform;
            prompt.SetActive(false);
        }
        else
        {
            panel.SetActive(false);
            freeLook.LookAt = focal.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = true;
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = false;
            prompt.SetActive(false);
            if (panel.activeSelf == true)
            {
                TogglePanel();
            }
        }
    }

}
