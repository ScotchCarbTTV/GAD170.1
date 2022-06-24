using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    //variable referencing the image component for the health
    [SerializeField] private Image healthIcon;
    //array of prefab images to use
    [SerializeField] private Sprite[] prefabs;
    //variable for storing the current health icon
    [SerializeField] private Sprite currentHealthIcon;


    private void Start()
    {
        //set the health icon to the full health icon
        SetHealthIcon(8);
    }

    public void SetHealthIcon(int health)
    {
        //set the currenHealthIcon to be equal to the prefab in the array at the same position as the player's current health
        currentHealthIcon = prefabs[health];
        //update the health icon to be the currenHealthIcon
        healthIcon.sprite = currentHealthIcon;
    }
}
