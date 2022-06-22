using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    //list of prefab images to use
    [SerializeField] private Image healthIcon;
    [SerializeField] private Sprite[] prefabs;
    [SerializeField] private Sprite currentHealthIcon;
   

    private void Start()
    {
        //set the health icon to the full health icon
        SetHealthIcon(8);
    }

    public void SetHealthIcon(int health)
    {
        currentHealthIcon = prefabs[health];
        healthIcon.sprite = currentHealthIcon;
    }
}
