using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //reference to the character controller
    [SerializeField] private PlayerControllerThatLevelsUp controller;

    //int to contain the player's maxHealth value
    [SerializeField] private int maxHealth;
    //int to contain the player's current health
    [SerializeField] public int currentHealth { get; private set; }

    //reference to the health icon script
    [SerializeField] private HealthDisplay hDisplay;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        if(!GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerControllerThatLevelsUp>(out controller))
        {
            Debug.LogError("You need to have a component with the tag 'Player'!");
            gameObject.SetActive(false);
        }        
    }

    public void LoseHealth()
    {
        //reduce player health by 1
        currentHealth--;
        Debug.Log(currentHealth);
        //update the player health UI element
        hDisplay.SetHealthIcon(currentHealth);        
    }

}
