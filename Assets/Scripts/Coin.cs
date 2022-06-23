using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Script: Coin
    Author: Gareth Lockett Pete Phillips
    Version: 2.0
    Description:    A simple script for a coin pickup. Make sure the coin object has a collider set to trigger.
                    The script spins/rotates the GameObject it is on.
                    When a GameObject, with a Rigidbody component, and tagged as Player enters the trigger, the player will get XP and this GameObject will be destroyed.

                    Note: This Script requires the PlayerControllerThatLevelsUp script.
*/

public class Coin : MonoBehaviour
{
    //variables for the spin value and xp value of the object
    public float spinSpeed = 100f;
    public int xpValue = 1;

    [SerializeField] private XPGainUI xpGainUI;

    public enum CoinType { OneXP, TenXP }
    public CoinType coinType;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("XPUIManager").TryGetComponent<XPGainUI>(out xpGainUI);
    }

    void Update()
    {
        //each frame rotate the object according to the spin speed modified by the framerate vs real time
        this.transform.Rotate(0f, Time.deltaTime * this.spinSpeed, 0f );
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if the object entering the trigger is the player
        if (other.gameObject.tag == "Player")
        {
            //call the GainXP method on the player
            other.gameObject.GetComponent<PlayerControllerThatLevelsUp>().GainXP(xpValue);
            //Debug.Log("The coin was collected");            
            if (coinType == CoinType.OneXP)
            {
                xpGainUI.OneXP();
            }
            else if(coinType == CoinType.TenXP)
            {
                xpGainUI.TenXP();
            }


            //remove this coin from the game scene
            Destroy(this.gameObject);
        }
    }
}