using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatManagerUI : MonoBehaviour
{
    //variables for the different stats as strings on screen
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI lockPickText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] TextMeshProUGUI jumpText;
    [SerializeField] TextMeshProUGUI currentXPText;
    [SerializeField] TextMeshProUGUI requiredXPText;

    public void UpdateTexts(float level, float lockPick, float speed,
        float turn, float jump, float current, float required)
    {
        levelText.text = "Level:\n" + level;
        lockPickText.text = "Lockpick skill:\n" + lockPick;
        speedText.text = "Move Speed:\n" + speed;
        turnText.text = "Turning Speed:\n" + turn;
        jumpText.text = "Jump Height:\n" + jump;
        currentXPText.text = "Current XP:\n" + current * 10;
        requiredXPText.text = "Next level:\n" + required * 10 + "xp";
    }
}
