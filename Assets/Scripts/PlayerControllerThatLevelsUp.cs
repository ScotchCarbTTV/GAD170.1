using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 
       Player Controller that moves and turns faster as it gains XP;
        Manages moving and rotating the player via the arrow keys.
        Up/down arrow keys to move forward and backward.
        Left/right arrow keys to rotate left and right.
*/

public class PlayerControllerThatLevelsUp : MonoBehaviour
{
    //variable for adjusting the rate at which the character falls
    public float gravityModifier = 2.5f;
    public float lowJumpMultipier = 2f;

    //The base move and turn speed
    public float moveSpeed = 1f;
    public float turnSpeed = 45f;
    public float jumpHeight = 2f;

    //The base lock picking skill which will determine how likely the player is to open a chest
    public int lockPickSkill;

    //The move and turn speed with the buffs you have from leveling up.   
    public float currentMoveSpeed;
    public float currentTurnSpeed;
    public float currentJumpHeight;

    //the lockpicking skill with the buff from levelling up
    public int currentLockPickSkill;


    public float xp = 0;    // Amount of XP the player has
    public float xpForNextLevel = 10;   //Xp needed to level up, the higher the level, the harder it gets. 
    public int level = 0;   // Level of the player

    [SerializeField] private Vector3 spawn;

    private AnimationManager animManager;

    private void Start()
    {        
        spawn = transform.position;
        
        SetXpForNextLevel();
        SetCurrentMoveSpeed();
        SetCurrentTurnSpeed();
        SetCurrentJumpHeight();
        SetCurrentLockPickSkill();

        animManager = GetComponent<AnimationManager>();
    }



    // To level up you need to collect an amount of xp;
    // This starts at 10 xp
    // Each level you gain the xp required gets higher exponentially
    // The exponential growth is slowed by scaling it by 10%

    void SetXpForNextLevel()
    {
        xpForNextLevel = (10f + (level * level * 0.1f));
        Debug.Log("xpForNextLevel " + xpForNextLevel);
    }

    // For each level, the player adds 10% to the move speed 
    void SetCurrentMoveSpeed()
    {
        currentMoveSpeed = this.moveSpeed + (this.moveSpeed * 0.1f * level);
        Debug.Log("currentMoveSpeed = " + currentMoveSpeed);
    }

    //For each level, the player will gain an additional 10% jump height. Very cool!
    void SetCurrentJumpHeight()
    {
        currentJumpHeight = this.jumpHeight + (this.jumpHeight * 0.1f * level);
        lowJumpMultipier = currentJumpHeight / 2;
        Debug.Log("currentJumpHeigh = " + currentJumpHeight);
    }

    // For each level, the player adds 10% to the turn speed 
    void SetCurrentTurnSpeed()
    {
        currentTurnSpeed = this.turnSpeed + (this.turnSpeed * (level * 0.1f));
        Debug.Log("currentTurnSpeed = " + currentTurnSpeed);
    }

    //For each level the player adds their current level to their initial lockPickSkill
    void SetCurrentLockPickSkill()
    {
        currentLockPickSkill = lockPickSkill + level;
    }

    void LevelUp()
    {
        xp = 0f;
        level++;
        Debug.Log("level" + level);
        SetXpForNextLevel();
        SetCurrentMoveSpeed();
        SetCurrentTurnSpeed();
        SetCurrentJumpHeight();
        //call the SetCurrentLockPickSkill method
        SetCurrentLockPickSkill();
    }

    //a function to make the player gain the amount of Xp the you tell it. 
    public void GainXP(int xpToGain)
    {
        xp += xpToGain;
        Debug.Log("Gained " + xpToGain + " XP, Current Xp = " + xp + ", XP needed to reach next Level = " + xpForNextLevel);
    }

    void Update()
    {
        //Test the GainXp function by pressing the x button. 
        if (Input.GetKeyDown(KeyCode.X) == true) { GainXP((int)xpForNextLevel); }

    //levelling up code
    #region
        //LevelUp when the appropriate conditions are met.
        if (xp >= xpForNextLevel)
        {
            LevelUp();
        }
    #endregion


        //movement: jumping, running, turning and falling code.
        #region
        // Check spacebar to trigger jumping. Checks if vertical velocity (eg velocity.y) is near to zero.
        if (Input.GetKeyDown(KeyCode.Space) == true && Mathf.Abs(this.GetComponent<Rigidbody>().velocity.y) < 0.01f && !animManager.fall)
        {
            this.GetComponent<Rigidbody>().velocity += Vector3.up * this.currentJumpHeight;
            animManager.JumpAnim();
        }
        //checks to see if the player is falling without having hit jump and triggers the falling animation 
        else if (Mathf.Abs(this.GetComponent<Rigidbody>().velocity.y) > 0.01f && animManager.fall == false)
        {
            animManager.JumpAnim();
        }
        //checks if the player is current descending and increases their fall rate by the gravityModifier
        if(this.GetComponent<Rigidbody>().velocity.y < 0 && animManager.fall == true)
        {
            this.GetComponent<Rigidbody>().velocity += Vector3.up * Physics.gravity.y * (gravityModifier - 1) * Time.deltaTime;
        }
        //checks if tthe player is still holding down spacebar; if not, the effect of gravity is increased so they perform a smaller 'hop'
        else if(Mathf.Abs(this.GetComponent<Rigidbody>().velocity.y) > 0.01f && !Input.GetKey(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().velocity += Vector3.up * Physics.gravity.y * (lowJumpMultipier - 1) * Time.deltaTime;
        }

        // Rotation and movement speed is modifed by the level (currentMoveSpeed) of the player and by the time between update frames (Time.deltaTime). 

        // Move player via up/down arrow keys
        
            if (Input.GetKey(KeyCode.UpArrow) == true)
            {
                this.transform.position += this.transform.forward * currentMoveSpeed * Time.deltaTime;
                if (animManager.fall == false)
                {
                    animManager.ToggleRun(true);
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow) == true)
            {
                this.transform.position -= this.transform.forward * currentMoveSpeed * Time.deltaTime;
                if (animManager.fall == false)
                {
                    animManager.ToggleRun(true);
                }
            }
            else
            {
                animManager.ToggleRun(false);
            }

       


        // Rotate player via left/right arrow keys
        // Identify this position, set the vertical axis as the axis to rotate around the set the rotation speed.
        if (Input.GetKey(KeyCode.RightArrow) == true) { this.transform.RotateAround(this.transform.position, Vector3.up, currentTurnSpeed * Time.deltaTime); }
        if (Input.GetKey(KeyCode.LeftArrow) == true) { this.transform.RotateAround(this.transform.position, Vector3.up, -currentTurnSpeed * Time.deltaTime); }
        #endregion
    }

    public void Respawn()
    {
        this.transform.position = spawn;
    }
}

