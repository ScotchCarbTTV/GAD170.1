using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestUI : MonoBehaviour
{
    //variable referencing the text on the object
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        //turn the canvas off when the game starts
        text.text = "Press Square/ Enter to Pick the Lock!";
        text.enabled = false;
    }


    public void LevelTooLow(int difficulty, int lockPick)
    {
        
        text.text = "Your Lockpick level is too low!\n" + "Chest difficulty: " + difficulty + "\nYour lockpick level: " + lockPick;
        StartCoroutine(ResetText());

    }

    //method to display a failed roll info
    public void FailedToOpen(int difficulty, int score)
    {
        
        text.text = "Failed roll!\n Chest difficulty: " + difficulty + "\nYour roll: " + score;
        StartCoroutine(ResetText());
    }

    public void Success()
    {        
        text.text = "Chest open!!!";
        StartCoroutine(ResetText());
    }

    public void ToggleText(bool toggle)
    {
        if(toggle == true)
        {
            text.enabled = true;
        }
        else
        {
            text.enabled = false;
        }
    }

    IEnumerator ResetText()
    {
        yield return new WaitForSeconds(3f);
        text.text = "Press Square/Enter to Pick the Lock!";
    }
}
